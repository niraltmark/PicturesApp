using System;

namespace SYW.App.Pictures.Domain.Services.Platform
{
    public class PlatformConfiguration : IPlatformConfiguration
    {
    	private readonly IPlatformIntegrationSettings _platformIntegrationSettings;

    	public PlatformConfiguration(IPlatformIntegrationSettings platformIntegrationSettings)
    	{
    		_platformIntegrationSettings = platformIntegrationSettings;
    	}

    	public string AppSecret
        {
			get { return _platformIntegrationSettings.AppSecret; }
        }

        public long AppId
        {
            get { return _platformIntegrationSettings.AppId; }
        }

        public Uri PlatformApiBaseUrl
        {
            get { return new Uri(_platformIntegrationSettings.PlatformApiBaseUrl); }
        }

        public Uri PlatformSecureApiBaseUrl
        {
            get { return new Uri(_platformIntegrationSettings.PlatformSecureApiBaseUrl); }
        }
    }
}