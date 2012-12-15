namespace SYW.App.Pictures.Models
{
	public class Panel
	{
		public PanelType Type { get; set; }
		public IPanelItem Item { get; set; }
	}

	public enum PanelType
	{
		Small = 0,
		Large = 1
	}
}