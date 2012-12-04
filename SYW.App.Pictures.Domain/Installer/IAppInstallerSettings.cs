using CommonGround.Settings;

namespace SYW.App.Pictures.Domain.Services.Installer
{
	public interface IAppInstallerSettings
	{
		[Default("4ffe85fc2cc2650c68b54f8f")]
		string SupportEntityId { get; set; }

		[Default("Welcome! How can I help you?")]
		string WelcomeMessage { get; set; }
	}
}