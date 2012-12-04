using System.Web;

namespace SYW.App.Messages.Web.Services
{
	public interface IHttpContextProvider
	{
		HttpContext GetContext();
	}

	public class HttpContextProvider : IHttpContextProvider
	{
		public HttpContext GetContext()
		{
			return HttpContext.Current;
		}
	}
}