using SYW.App.Pictures.Domain.Settings;

namespace SYW.App.Pictures.Domain
{
	public interface IRoutes
	{
		string DefaultAppUrl { get; }
	}

	public class Routes : IRoutes
	{
		private readonly IApplicationSettings _applicationSettings;

		public Routes(IApplicationSettings applicationSettings)
		{
			_applicationSettings = applicationSettings;
		}

		public string DefaultAppUrl
		{
			get { return string.Format(_applicationSettings.DefaultAppUrl, _applicationSettings.AppId); }
		}
	}
}