using System;
using System.Collections.Specialized;
using CommonGround.Utilities;
using HelloApps.Services;
using SYW.App.Pictures.Domain.Settings;

namespace SYW.App.Pictures.Domain.Services.Platform
{
    public interface IAuthService
    {
        string GetOfflineToken(long userId);
    }

    public class AuthService : IAuthService
    {
        private readonly IPlatformProxy _platformProxy;
        private readonly IApplicationSettings _applicationSettings;
        private readonly string _servicePath;

        public AuthService(IPlatformProxy platformProxy, IApplicationSettings applicationSettings)
        {
            _platformProxy = platformProxy;
            _applicationSettings = applicationSettings;
        	_servicePath = _applicationSettings.AuthenticationPath;
        }

        public string GetOfflineToken(long userId)
        {
        	var now = SystemTime.Now();

            var timeStamp = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, DateTimeKind.Utc);

			var signature = new SignatureBuilder().Append(userId).Append(_applicationSettings.AppId).Append(timeStamp).Append(_applicationSettings.AppSecret).Sign();

            var serviceParameters = new NameValueCollection(4) {
            	{"userId", userId.ToString()},
            	{"appId", _applicationSettings.AppId.ToString()},
            	{"timestamp", timeStamp.ToString("yyyy-MM-ddTHH:mm:ss")},
            	{"signature", signature}
            };

        	return _platformProxy.GetOfflineToken(_servicePath + "/get-token", serviceParameters);
        }
    }
}