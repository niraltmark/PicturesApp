using PlatformClient.Platform;
using SYW.App.Pictures.Domain.Services.Settings;

namespace SYW.App.Pictures.Domain.Services.Platform
{
    public interface IAppService
    {
        void Install();
        void Uninstall();
    }

    public class AppService : IAppService
    {
        private readonly IPlatformProxy _platformProxy;
        private readonly IApplicationSettings _applicationSettings;
        private readonly string _servicePath;

        public AppService(IPlatformProxy platformProxy, IApplicationSettings applicationSettings)
        {
            _platformProxy = platformProxy;
            _applicationSettings = applicationSettings;
            _servicePath = _applicationSettings.AppPath;
        }

        public void Install()
        {
            _platformProxy.Get<string>(_servicePath + "/install");
        }

        public void Uninstall()
        {
            _platformProxy.Get<string>(_servicePath + "/uninstall");
        }
    }
}