using System.Web;
using SYW.App.Messages.Web.Cookies;

namespace SYW.App.Pictures.Web.Cookies
{
	public abstract class ContextAwareComponent
	{
		protected ContextAwareComponent(IEngineContextLocator contextLocator)
		{
			ContextLocator = contextLocator;
		}

		protected IEngineContextLocator ContextLocator { get; set; }

		protected HttpContextBase Context
		{
			get { return ContextLocator.LocateCurrentContext(); }
		}
	}
}