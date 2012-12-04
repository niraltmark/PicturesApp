using System;
using CommonGround.Utilities;
using SYW.App.Pictures.Services;

namespace SYW.App.Messages.Web.Services
{
	public static class DateTimeUIExt
	{
		private const int SixtySeconds = 60;
		private const int Minute = 1;
		private const int Hour = Minute * 60;
		private const int Day = Hour * 24;
		private const int TwoDays = 2 * Day;
		private const string MinutesAgoText = " minutes ago";
		private const string OneMinuteAgoText = "1 minute ago";
		private const string OneHourAgoText = "1 hour ago";
		private const string HoursAgoText = " hours ago";
		private const string YesterdayAtText = "yesterday at ";
		private const string AboutText = "about ";
		private const string OnText = "on ";
		private const string TodayText = "today";
		private const string YesterdayText = "yesterday";
		private static readonly string[] Months = new[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };

		public static IUserTimeCookiedOffsetResolver TimeOffsetResolver;

		/// <summary>
		/// According to spec Common UI elements (Real-time object).
		/// This method assumes that both the input and "now" are synced to UTC time.
		/// </summary>
		public static string ToRealTimeString(this DateTime value)
		{
			DateTime now = TimeOffsetResolver.Resolve(SystemTime.Now());
			DateTime usersValue = TimeOffsetResolver.Resolve(value);

			if (value > now)
				return OneMinuteAgoText;

			TimeSpan timeDiff = (now - usersValue);

			if (timeDiff.TotalSeconds <= SixtySeconds)
				return OneMinuteAgoText;
			if (timeDiff.TotalMinutes == Hour)
				return OneHourAgoText;

			if (timeDiff.TotalMinutes < Hour)
			{
				var minutes = Math.Ceiling(timeDiff.TotalMinutes);
				return (minutes == 60) ? OneHourAgoText : minutes + MinutesAgoText;
			}

			if (timeDiff.TotalMinutes < Day)
			{
				var hours = Math.Round(timeDiff.TotalMinutes / 60, 0);
				if (hours == 1)
					return AboutText + " " + OneHourAgoText;

				return (hours == 24) ? YesterdayAtText + GetUSTime(usersValue) : AboutText + hours + HoursAgoText;
			}

			if (IsYesterday(now, value))
				return YesterdayAtText + GetUSTime(usersValue);

			if (timeDiff.TotalDays < 30)
				return timeDiff.Days + " days ago";

			if (value.Year == now.Year)
				return GetUSMonthNameAndDay(usersValue);

			return GetUSMonthNameAndDay(usersValue) + ", " + usersValue.Year;
		}

		/// <summary>
		/// According to spec Common UI elements (Non Real-time object).
		/// This method assumes that both the input and "now" are synced to UTC time.
		/// </summary>
		public static string ToNonRealTimeString(this DateTime value)
		{
			return ToNonRealTimeString(value, true);
		}

		public static string ToNonRealTimeString(this DateTime value, bool withOnText)
		{
			DateTime now = SystemTime.Now();
			DateTime usersValue = TimeOffsetResolver.Resolve(value);

			if (value > now)
				return TodayText;

			if (now.Year == value.Year && now.Month == value.Month && now.Day == value.Day)
				return TodayText;

			if (IsYesterday(now, value))
				return YesterdayText;

			var onText = withOnText ? OnText : string.Empty;

			if (value.Year == now.Year)
				return onText + GetUSMonthNameAndDay(usersValue);

			return onText + GetUSMonthNameAndDay(usersValue) + ", " + usersValue.Year;
		}

		public static string ToUpcomingBirthdayString(this DateTime value)
		{
			if (value.IsToday())
				return TodayText;

			return GetUSMonthNameAndDay(value);
		}

		public static bool IsToday(this DateTime value)
		{
			var now = SystemTime.Now();

			return (now.Year == value.Year && now.Month == value.Month && now.Day == value.Day);
		}

		/// <summary>
		/// Renders a date as yyyy-MM-ddTHH:mm:ssZ, which will work across platforms, and still be human readable
		/// <example>2009-09-29T18:42:00Z, for Sep. 29th, 18:42 in UTC time</example>
		/// </summary>
		/// <param name="value">The date to render</param>
		/// <returns>A human readable, cross platform representation of <paramref name="value"/></returns>
		public static string ToIso8601String(this DateTime value)
		{
			return value.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
		}

		public static string ToFullBirthdayForm(this DateTime value)
		{
			return value.ToString("MMMM d, yyyy", System.Globalization.CultureInfo.GetCultureInfo("en-us"));
		}

		public static string ToTimeOnly(this DateTime value)
		{
			return GetUSTime(TimeOffsetResolver.Resolve(value));
		}

		public static string ToMonthAndDayString(this DateTime value)
		{
			return GetUSMonthNameAndDay(value);
		}

		public static string ToUserLongDateTimeString(this DateTime value)
		{
			return TimeOffsetResolver.Resolve(value).ToString("MMMM d, yyyy a\\t H:mm:ss");
		}

		private static string GetUSMonthNameAndDay(DateTime value)
		{
			// pasha wants english format of month day (not by thread culture)
			return Months[value.Month - 1] + value.ToString(" d") + CalcDaySuffix(value.Day);
		}

		private static bool IsYesterday(DateTime now, DateTime original)
		{
			TimeSpan timeDiff = (now - original);

			if (timeDiff.TotalMinutes < TwoDays)
			{
				DateTime yesterday = now.AddDays(-1);
				if (yesterday.Year == original.Year && yesterday.Month == original.Month && yesterday.Day == original.Day)
					return true;
			}

			return false;
		}

		private static string CalcDaySuffix(int day)
		{
			if (day == 1 || day == 21 || day == 31)
				return "st";
			if (day == 2 || day == 22)
				return "nd";
			if (day == 3 || day == 23)
				return "rd";

			return "th";
		}

		private static string GetUSTime(DateTime value)
		{
			return value.ToString("h:mmtt").ToLower();
		}
	}
}