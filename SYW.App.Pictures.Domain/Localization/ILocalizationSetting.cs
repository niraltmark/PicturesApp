using CommonGround.Settings;

namespace SYW.App.Pictures.Domain.Services.Localization
{
	public interface ILocalizationSettings
	{
		[Default("messages_locale_tz")]
		string TimeZoneCookieName { get; set; }

		[Default("messages_locale_dl")]
		string DefaultLanguageCookieName { get; set; }
	}
}