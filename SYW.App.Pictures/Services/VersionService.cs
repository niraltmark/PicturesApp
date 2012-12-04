using System.Reflection;

namespace SYW.App.Messages.Web.Services
{
	public interface IVersionService
	{
		string GetVersionTag();
	}

	public class VersionService : IVersionService
	{
		private readonly string _versionTag;

		public VersionService()
		{
			_versionTag = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			_versionTag = _versionTag.Replace('.', '-');
		}

		public string GetVersionTag()
		{
			return _versionTag;
		}
	}
}