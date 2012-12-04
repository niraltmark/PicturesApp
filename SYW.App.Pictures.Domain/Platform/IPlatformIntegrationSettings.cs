using CommonGround.Settings;

namespace SYW.App.Pictures.Domain.Services.Platform
{
	public interface IPlatformIntegrationSettings
	{
		// 147: 89FD735F2D1E47abB2533C499CA9A236
		// 144: 0734E51010E94f5d8260366743FBCA9C

		[Default("0734E51010E94f5d8260366743FBCA9C")]
		string AppSecret { get; set; }

		[Default(1000)]
		int AppId { get; set; }

		[Default("http://ohio.platform:88")]
		string PlatformApiBaseUrl { get; set; }

		[Default("http://ohio.platform:88")]
		string PlatformSecureApiBaseUrl { get; set; }
	}
}