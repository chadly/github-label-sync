using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LabelSync
{
	class Program
	{
		static async Task Main(string[] args)
		{
			if (args.Length != 1 && args.Length != 2)
			{
				Console.WriteLine("Usage: sync [github-api-token] <remove-labels>");
				return;
			}

			bool removeLabels = args.Length == 2 && args[1] == "remove-labels";

			using (var github = CreateGithubClient(args[0]))
			{
				var repos = await github.GetReposForOrg("runlyio");

				Console.WriteLine($"Retrieved {repos.Count()} respositories");
				Console.WriteLine();

				foreach (var repo in repos)
				{
					if (repo.Archived)
					{
						Console.WriteLine($"{repo.FullName}: Archived; skipping");
					}
					else
					{
						var existingLabels = await github.GetLabelsForRepo(repo.FullName);
						var existingLabelNames = existingLabels.Select(l => l.Name);
						var templateLabelNames = Label.All.Select(l => l.Name);

						if (removeLabels)
						{
							var labelsToRemove = existingLabelNames.Except(templateLabelNames).Where(l => !l.StartsWith("deploy-") && !l.StartsWith("deployment-"));

							Console.WriteLine($"{repo.FullName}: Removing {labelsToRemove.Count()} labels");
							foreach (string lbl in labelsToRemove)
								await github.DeleteLabel(repo.FullName, lbl);
						}

						var labelsToAdd = Label.All.Where(l => !existingLabelNames.Contains(l.Name));

						Console.WriteLine($"{repo.FullName}: Adding {labelsToAdd.Count()} labels");
						foreach (var lbl in labelsToAdd)
							await github.CreateLabel(repo.FullName, lbl.Name, lbl.Color, lbl.Description);

						var labelsToUpdate = Label.All.Where(l =>
						{
							var existing = existingLabels.SingleOrDefault(el => el.Name == l.Name);
							if (existing == null)
								return false;

							return existing.Color != l.Color || existing.Description != l.Description;
						});

						Console.WriteLine($"{repo.FullName}: Updating {labelsToUpdate.Count()} labels");
						foreach (var lbl in labelsToUpdate)
							await github.UpdateLabel(repo.FullName, lbl.Name, lbl.Name, lbl.Color, lbl.Description);
					}

					Console.WriteLine();
				}
			}
		}

		static HttpClient CreateGithubClient(string apiToken)
		{
			var client = new HttpClient()
			{
				BaseAddress = new Uri("https://api.github.com")
			};

			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(Api.MediaType));
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("token", apiToken);
			client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("github-label-sync", "v1.0.0"));

			return client;
		}
	}
}
