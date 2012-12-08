using System.Web;
using System.Web.Mvc;
using CommonGround.MvcInvocation;
using SYW.App.Pictures.Domain.Installer;
using SYW.App.Pictures.Web.Filters;

namespace SYW.App.Pictures.Controllers
{
	public class HomePageController : PicturesControllerBase
	{
		private readonly IAppInstaller _appInstaller;

		public HomePageController(IAppInstaller appInstaller)
		{
			_appInstaller = appInstaller;
		}

		[PatternRoute("/")]
		public ActionResult Index()
		{
			return View();
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