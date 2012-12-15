using System.Collections.Generic;

namespace SYW.App.Pictures.Models
{
	public class Grid
	{
		public Grid()
		{
			Blocks = new List<Block>();
		}

		public IList<Block> Blocks { get; set; }
	}
}