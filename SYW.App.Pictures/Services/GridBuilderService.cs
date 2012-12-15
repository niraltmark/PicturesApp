using System;
using System.Collections.Generic;
using SYW.App.Pictures.Domain.Entities;
using SYW.App.Pictures.Models;
using System.Linq;
using SYW.App.Pictures.Domain;

namespace SYW.App.Pictures.Services
{
	public interface IGridBuilderService
	{
		Grid Build(IList<IPanelItem> items);
	}

	public class GridBuilderService : IGridBuilderService
	{
		public Grid Build(IList<IPanelItem> items)
		{
			var grid = new Grid();

			var i = 1;

			while (!items.IsNullOrEmpty())
			{
				grid.Blocks.Add(GenerateBlock(items.Take(i)));

				items = items.Skip(i).ToList();

				if (items.IsNullOrEmpty())
					break;

				i = i == 1 ? 4 : 1;

				grid.Blocks.Add(GenerateBlock(items.Take(i)));

				items = items.Skip(i).ToList();
			}

			return grid;
		}

		private Block GenerateBlock(IEnumerable<IPanelItem> items)
		{
			var block = new Block {Type = BlockType.Large};
			block.Panels.AddRange(items.Select(i => new Panel {Item = i, Type = items.Count() == 1 ? PanelType.Large : PanelType.Small}).ToList());
			return block;
		}
	}
}