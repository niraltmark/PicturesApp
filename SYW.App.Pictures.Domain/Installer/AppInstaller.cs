using SYW.App.Pictures.Domain.Services;
using SYW.App.Pictures.Domain.Services.Platform;
using log4net;

namespace SYW.App.Pictures.Domain.Installer
{
	public interface IAppInstaller
	{
		void Install();
	}

	public class AppInstaller : IAppInstaller
	{
		private readonly IAppLinksService _appLinksService;
		private readonly IAppService _appService;
		private readonly ILog _logger;
		private readonly IRoutes _routes;

		public AppInstaller(IAppService appService, ILog log, IAppLinksService appLinksService,  IRoutes routes)
		{
			_appService = appService;
			_logger = log;
			_appLinksService = appLinksService;
			_routes = routes;
		}

		public void Install()
		{
			_logger.Debug("Entering Post-Login");

			_appService.Install();

			_logger.Debug("App installed");

			_appLinksService.Register("Pictures", _routes.DefaultAppUrl);

			_logger.Debug("App registered");
		}
	}
}	
