using System.Web;
using System.Web.Mvc;
using CommonGround.MvcInvocation;

namespace SYW.App.Pictures.Controllers
{
	public class HomePageController : PicturesControllerBase
	{
		[PatternRoute("/")]
		public ActionResult Index()
		{
			return Content("Welcome to my products and I");
		}

		[PatternRoute("/post-login")]
		public ActionResult PostLogin()
		{
			return Content("Yes");
		}
	}
}