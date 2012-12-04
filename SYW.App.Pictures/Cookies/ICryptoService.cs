using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using PlatformClient;

namespace SYW.App.Messages.Web.Cookies
{
	public interface ICryptoService
	{
		byte[] Hash(byte[] input);
		byte[] Sign(byte[] input);
		string Sign(string text);

		bool VerifySignature(string text, string signature);
	}

	public class CryptoService : ICryptoService
	{
		private readonly HashAlgorithm _hashAlgorithm;

		public CryptoService(ICryptoSettings settings)
		{
			_hashAlgorithm = SHA256.Create();

			if (settings.EncryptionKey == null || settings.EncryptionKey.Length != 64)
				throw new ConfigurationErrorsException("CryptoService.EncryptionKey is missing in the configuration table or corrupt! It should be a 64 char length hex representation of a 32 byte array (256 bit), try redeploying the sharvrir or reimporting the schema. Key was: " + (string.IsNullOrEmpty(settings.EncryptionKey) ? "null/empty" : settings.EncryptionKey));

			EncryptionKey = settings.EncryptionKey.FromHexString();
		}

		private byte[] EncryptionKey { get; set; }

		public byte[] Hash(byte[] input)
		{
			return _hashAlgorithm.ComputeHash(input);
		}

		public byte[] Sign(byte[] input)
		{
			return Hash(Concat(input, EncryptionKey));
		}

		public string Sign(string text)
		{
			return Convert.ToBase64String(Sign(Encoding.UTF8.GetBytes(text)));
		}

		public bool VerifySignature(string text, string signature)
		{
			return Sign(text) == signature;
		}

		private byte[] Concat(byte[] first, byte[] second)
		{
			var result = new byte[first.Length + second.Length];
			Buffer.BlockCopy(first, 0, result, 0, first.Length);
			Buffer.BlockCopy(second, 0, result, first.Length, second.Length);
			return result;
		}
	}
}