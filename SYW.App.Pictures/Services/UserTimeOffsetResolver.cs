using System;
using System.Web;
using SYW.App.Pictures.Domain.Services.Localization;

namespace SYW.App.Pictures.Services
{
	public interface IUserTimeCookiedOffsetResolver
	{
		DateTime Resolve(DateTime utcDate);
	}

	public class UserTimeCookiedOffsetResolver : IUserTimeCookiedOffsetResolver
	{
		private readonly ILocalizationSettings _localizationSettings;

		public UserTimeCookiedOffsetResolver(ILocalizationSettings localizationSettings)
		{
			_localizationSettings = localizationSettings;
		}

		private double CookieOffsetValue
		{
			get
			{
				var offset = 0.0;

				if (HttpContext.Current == null)
					return offset;

				HttpCookie cookie = HttpContext.Current.Request.Cookies[_localizationSettings.TimeZoneCookieName];
				if (cookie == null)
					return offset;

				if (!double.TryParse(cookie.Value, out offset))
					offset = 0.0;

				return offset;
			}
		}

		public DateTime Resolve(DateTime utcDate)
		{
			var offset = CookieOffsetValue;
			DateTime userLocalTime = utcDate + ToTimeSpan(offset);
			return userLocalTime;
		}

		private TimeSpan ToTimeSpan(double offset)
		{
			int hours = offset < 0 ? (int)Math.Ceiling(offset) : (int)Math.Floor(offset);
			int minutes = (int)((offset - hours) * 60);
			return new TimeSpan(hours, minutes, 0);
		}
	}
}