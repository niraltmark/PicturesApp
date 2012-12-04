using CommonGround.Settings;

namespace SYW.App.Pictures.Domain.Services.Platform
{
	public interface IPlatformIntegrationSettings
	{
		// 147: 89FD735F2D1E47abB2533C499CA9A236
		// 144: 0734E51010E94f5d8260366743FBCA9C

		[Default("f6c0df3aaa854e4d98ef36bee80ffcd7")]
		string AppSecret { get; set; }

		[Default(1725)]
		int AppId { get; set; }

		[Default("http://ohio.platform:88")]
		string PlatformApiBaseUrl { get; set; }

		[Default("http://ohio.platform:88")]
		string PlatformSecureApiBaseUrl { get; set; }
	}
}