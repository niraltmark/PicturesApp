using System.Security.Cryptography;
using System.Text;
using PlatformClient;

namespace SYW.App.Pictures.Domain.Services.Platform
{
    public interface IPlatformHashProvider
    {
        string GetHash();
    }

    public class PlatformHashProvider : IPlatformHashProvider
    {
        private readonly IPlatformConfiguration _platformConfiguration;
        private readonly IPlatformTokenProvider _platformTokenProvider;

        private readonly HashAlgorithm _hashAlgorithm = SHA256.Create();

        public PlatformHashProvider(IPlatformConfiguration platformConfiguration, IPlatformTokenProvider platformTokenProvider)
        {
            _platformConfiguration = platformConfiguration;
            _platformTokenProvider = platformTokenProvider;
        }

        public string GetHash()
        {
            var saltedWithSecret = Encoding.UTF8.GetBytes(_platformTokenProvider.GetToken() + _platformConfiguration.AppSecret);

            return _hashAlgorithm.ComputeHash(saltedWithSecret).ToHexString().ToLower();
        }
    }
}