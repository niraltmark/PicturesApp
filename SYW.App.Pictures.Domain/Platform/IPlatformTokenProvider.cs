using PlatformClient.Platform;

namespace SYW.App.Pictures.Domain.Services.Platform
{
    public interface IPlatformTokenProvider
    {
        string GetToken();
        void SetToken(string token);
    }

    public class PlatformTokenProvider : IPlatformTokenProvider
    {
        private const string TokenKey = "token";

        private readonly IHttpContextState _httpContextState;

        public PlatformTokenProvider(IHttpContextState httpContextState)
        {
            _httpContextState = httpContextState;
        }

        public string GetToken()
        {
            return _httpContextState.GetItem<string>(TokenKey);
        }

        public void SetToken(string token)
        {
            _httpContextState.SetItem(TokenKey, token);
        }
    }
}