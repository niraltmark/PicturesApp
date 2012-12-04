using System;

namespace SYW.App.Pictures.Domain.Services.Platform
{
    public interface IPlatformConfiguration
    {
         string AppSecret { get; }
         long AppId { get; }
         Uri PlatformApiBaseUrl { get; }
		 Uri PlatformSecureApiBaseUrl { get; }
    }
}