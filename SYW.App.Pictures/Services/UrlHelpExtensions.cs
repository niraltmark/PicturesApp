using System.Web.Mvc;

namespace SYW.App.Messages.Web.Services
{
	public static class UrlHelpExtensions
	{
		public static IVersionService VersionService;

		public static string VersionedContent(this UrlHelper urlHelper, string contentPath)
		{
			var result = urlHelper.Content(contentPath);

			var tag = VersionService.GetVersionTag();

			return result + (result.Contains("?") ? "&v=" : "?v=") + tag;
		}
	}
}