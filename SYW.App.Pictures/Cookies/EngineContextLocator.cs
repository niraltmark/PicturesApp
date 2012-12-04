using System.Web;

namespace SYW.App.Messages.Web.Cookies
{
	public interface IEngineContextLocator
	{
		HttpContextBase LocateCurrentContext();
	}

	public class EngineContextLocator : IEngineContextLocator
	{
		public HttpContextBase LocateCurrentContext()
		{
			return new HttpContextWrapper(HttpContext.Current);
		}
	}
}