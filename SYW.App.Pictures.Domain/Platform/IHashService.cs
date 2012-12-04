using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using PlatformClient;
using SYW.App.Pictures.Domain.Services.Platform;

namespace HelloApps.Services
{
    public interface IHashService
    {
        string CreateSignature(long userId, long appId, string appSecret, DateTime now);
        string CreateSignature(string token, string appSecret);
    }

    public class HashService : IHashService
    {
        private readonly SHA256 _hashAlgorithm;

        public HashService()
        {
            _hashAlgorithm = SHA256.Create();
        }

        public string CreateSignature(long userId, long appId, string appSecret, DateTime now)
        {
            var bytes = BitConverter.GetBytes(userId)
                .Concat(BitConverter.GetBytes(appId))
                .Concat(BitConverter.GetBytes(now.ToUnixTime()))
                .Concat(Encoding.UTF8.GetBytes(appSecret))
                .ToArray();

            return _hashAlgorithm.ComputeHash(bytes).ToHexString().ToLower();
        }

        public string CreateSignature(string token, string appSecret)
        {
            var tokenSecret = Encoding.UTF8.GetBytes(token + appSecret);

            return _hashAlgorithm.ComputeHash(tokenSecret).ToHexString().ToLower();
        }
    }
}