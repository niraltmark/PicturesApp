using CommonGround.Settings;

namespace SYW.App.Pictures.Domain.Services.Settings
{
    public interface IApplicationSettings
    {
		[Default("0734E51010E94f5d8260366743FBCA9C")]
        string AppSecret { get; set; }

        [Default("1000")]
        long AppId { get; set; }

        [Default("applinks")]
        string AppLinkPath { get; set; }

        [Default("app")]
        string AppPath { get; set; }

        [Default("tags")]
        string TagsPath { get; set; }

        [Default("users")]
        string UsersPath { get; set; }

        [Default("wall")]
        string WallPath { get; set; }

        [Default("auth")]
        string AuthenticationPath { get; set; }

        [Default("messages")]
        string Domain { get; set; }

        [Default("8")]
        int ListPageSize { get; set; }

        [Default("4")]
        int GridPageSize { get; set; }

        [Default("24")]
        int PackagePageSize { get; set; }

        [Default("6")]
        int PackageRowSize { get; set; }

        [Default("http://static.shopyourway.com/static/messages")]
        string StaticContentLocation { get; set; }

		[Default("http://ohio.local")]
		string BaseSiteUrl { get; set; }

		[Default("/app/{0}/r")]
		string DefaultAppUrl { get; set; }

	}
   
}