using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CommonGround;
using SYW.App.Pictures.Config;

namespace SYW.App.Pictures
{
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : System.Web.HttpApplication
	{
		private ICommonContainer _container;

		protected void Application_Start()
		{
			Initialize();
		}

		private void Initialize()
		{
			_container = new CommonContainer();
			var bootstrapper = new Bootstrapper(_container);
			bootstrapper.RegisterEverything();
		}

		protected void Application_EndRequest()
		{
			// MiniProfiler.Stop();
		}
	}
}