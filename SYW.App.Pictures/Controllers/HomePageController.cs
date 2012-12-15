using System.Linq;
using System.Web;
using System.Web.Mvc;
using CommonGround.MvcInvocation;
using SYW.App.Pictures.Domain.DataAccess;
using SYW.App.Pictures.Domain.Installer;
using SYW.App.Pictures.Models;
using SYW.App.Pictures.Services;
using SYW.App.Pictures.Web.Filters;

namespace SYW.App.Pictures.Controllers
{
	public class HomePageController : PicturesControllerBase
	{
		private readonly IAppInstaller _appInstaller;
		private readonly IGridBuilderService _gridBuilderService;
		private readonly IPicuturesRespository _picuturesRespository;

		public HomePageController(IAppInstaller appInstaller, IGridBuilderService gridBuilderService, IPicuturesRespository picuturesRespository)
		{
			_appInstaller = appInstaller;
			_gridBuilderService = gridBuilderService;
			_picuturesRespository = picuturesRespository;
		}

		[PatternRoute("/")]
		public ActionResult Index()
		{
			var pictures = _picuturesRespository.Get(50);

			var grid = _gridBuilderService.Build(pictures.Select(p => new PanelItem {ImageUrl = p.ImageUrl}).ToArray());
			
			return View(new PicturePageModel
			{
				Grid = grid
			});
		}

		[IgnoreAutoLogin]
		[PatternRoute("/post-login")]
		public ActionResult PostLogin()
		{
			_appInstaller.Install();

			return Content("Yes");
		}
	}
}