using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Archon.Http;
using Newtonsoft.Json;

namespace LabelSync
{
	public static class Api
	{
		public const string MediaType = "application/vnd.github.symmetra-preview+json"; //"application/vnd.github.v3+json";

		public static Task<IEnumerable<Repository>> GetReposForOrg(this HttpClient github, string org, string type = "all")
		{
			return github.AggregatePaginatedResponse<Repository>($"/orgs/{org}/repos?type={type}");
		}

		public static Task<IEnumerable<Label>> GetLabelsForRepo(this HttpClient github, string repo)
		{
			return github.AggregatePaginatedResponse<Label>($"/repos/{repo}/labels");
		}

		public static async Task<Label> CreateLabel(this HttpClient github, string repo, string name, string color, string description)
		{
			var response = await github.PostAsJsonAsync($"/repos/{repo}/labels", new { name, color, description });
			await response.EnsureSuccess();

			return await response.Content.ReadAsAsync<Label>();
		}

		public static async Task<Label> UpdateLabel(this HttpClient github, string repo, string oldName, string newName, string color, string description)
		{
			var response = await github.PatchAsJsonAsync($"/repos/{repo}/labels/{oldName}", new { name = newName, color, description });
			await response.EnsureSuccess();

			return await response.Content.ReadAsAsync<Label>();
		}

		public static async Task DeleteLabel(this HttpClient github, string repo, string name)
		{
			var response = await github.DeleteAsync($"/repos/{repo}/labels/{name}");
			await response.EnsureSuccess();
		}

		static async Task<IEnumerable<T>> AggregatePaginatedResponse<T>(this HttpClient github, string url)
		{
			var results = new List<T>();

			string nextPageLink = url;

			while (nextPageLink != null)
			{
				var response = await github.GetAsync(nextPageLink);
				await response.EnsureSuccess();

				results.AddRange(await response.Content.ReadAsAsync<IEnumerable<T>>());

				nextPageLink = ParseNextPageLink(response.Headers);
			}

			return results;
		}

		static string ParseNextPageLink(HttpResponseHeaders headers)
		{
			IEnumerable<string> headerLinks;
			if (!headers.TryGetValues("Link", out headerLinks))
				return null;

			string links = headerLinks.SingleOrDefault();
			if (String.IsNullOrWhiteSpace(links))
				return null;

			string[] parts = links.Split(',');

			string nextLink = parts.SingleOrDefault(p => p.Contains("rel=\"next\""));
			if (String.IsNullOrWhiteSpace(nextLink))
				return null;

			if (!nextLink.Contains("<") && !nextLink.Contains(">"))
				return null;

			nextLink = nextLink.Trim();

			return nextLink.Substring(1, nextLink.IndexOf(">") - 1);
		}

		static Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
		{
			var content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, MediaType);
			var request = new HttpRequestMessage(HttpMethod.Post, requestUri) { Content = content };

			return client.SendAsync(request);
		}

		static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
		{
			var content = new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, MediaType);
			var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = content };

			return client.SendAsync(request);
		}

		static async Task<T> ReadAsAsync<T>(this HttpContent content)
		{
			string json = await content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(json);
		}
	}
}
