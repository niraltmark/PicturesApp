using CommonGround.Settings;

namespace SYW.App.Messages.Web.Services
{
	public interface ISearchServiceSettings
	{
		[Default(10)]
		int MaxFriendsSearchResults { get; set; }
	}
}