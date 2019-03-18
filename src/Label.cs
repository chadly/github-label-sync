using System.Collections.Generic;

namespace Archon.LabelSync
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
					Description = "That shit you think is working...it broke",
					Color = "d73a4a"
				};

				yield return new Label
				{
					Name = "explore",
					Description = "Come with me on an adventure of exploring",
					Color = "f9e6c0"
				};

				yield return new Label
				{
					Name = "feature",
					Description = "WE NEED ALL THE FEATUREZ",
					Color = "05bafc"
				};

				yield return new Label
				{
					Name = "improvement",
					Description = "Not a feature, not a bug, just something that will maybe might make things better?",
					Color = "0e8a16"
				};

				yield return new Label
				{
					Name = "performance",
					Description = "Make things faster...but not too fast",
					Color = "8f62ba"
				};

				yield return new Label
				{
					Name = "refactor",
					Description = "Change everything...but not really",
					Color = "f4afa6"
				};

				yield return new Label
				{
					Name = "task",
					Description = "Just some shit that somebody's gotta do",
					Color = "bfd4f2"
				};

				yield return new Label
				{
					Name = "epic",
					Description = "BIG...so big",
					Color = "2b7696"
				};
			}
		}
	}
}
