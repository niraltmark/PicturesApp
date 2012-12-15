namespace SYW.App.Pictures.Models
{
	public interface IPanelItem
	{
		string ImageUrl { get; set; }
	}

	public class PanelItem : IPanelItem
	{
		public string ImageUrl { get; set; }
	}
}