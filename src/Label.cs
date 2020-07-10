using System.Collections.Generic;

namespace LabelSync
{
	public class Label
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Color { get; set; }

		public override string ToString() => Name;

		public static IEnumerable<Label> All
		{
			get
			{
				yield return new Label
				{
					Name = "bug",
					Description = "Something isn't working",
					Color = "d73a4a"
				};

				yield return new Label
				{
					Name = "duplicate",
					Description = "This issue or pull request already exists",
					Color = "cfd3d7"
				};

				yield return new Label
				{
					Name = "enhancement",
					Description = "New feature or request",
					Color = "a2eeef"
				};

				yield return new Label
				{
					Name = "invalid",
					Description = "This doesn't seem right",
					Color = "e4e669"
				};

				yield return new Label
				{
					Name = "maintenance",
					Description = "Project maintenance or refactoring that adds no features or bug fixes.",
					Color = "fff59a"
				};

				yield return new Label
				{
					Name = "major",
					Description = "PRs with breaking changes requiring a major version bump according to SemVer",
					Color = "b22f21"
				};

				yield return new Label
				{
					Name = "minor",
					Description = "PRs with new features requiring a minor version bump according to SemVer",
					Color = "f69448"
				};

				yield return new Label
				{
					Name = "patch",
					Description = "PRs with bug fixes requiring a patch version bump according to SemVer",
					Color = "eaf42c"
				};

				yield return new Label
				{
					Name = "question",
					Description = "Further information is requested",
					Color = "d876e3"
				};

				yield return new Label
				{
					Name = "wontfix",
					Description = "This will not be worked on",
					Color = "ffffff"
				};
			}
		}
	}
}
