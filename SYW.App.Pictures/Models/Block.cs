using System.Collections.Generic;

namespace SYW.App.Pictures.Models
{
	public class Block
	{
		public Block()
		{
			Panels = new List<Panel>();
		}

		public BlockType Type { get; set; }
		public List<Panel> Panels { get; set; }
	}

	public enum BlockType
	{
		Small = 0,
		Large = 1
	}
}