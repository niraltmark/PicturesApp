using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using PlatformClient;

namespace SYW.App.Pictures.Domain.Services.Platform
{
	public class SignatureBuilder : ISignatureBuilder
	{
		private readonly SHA256 _hashAlgorithm;
		private IEnumerable<byte> _bytes = Enumerable.Empty<byte>();

		public SignatureBuilder()
		{
			_hashAlgorithm = SHA256.Create();
		}

		public ISignatureBuilder Append(IEnumerable<byte> bytes)
		{
			if (bytes.IsNullOrEmpty()) throw new ArgumentNullException("bytes");

			_bytes = _bytes.Concat(bytes);

			return this;
		}

		public string Sign()
		{
			return _hashAlgorithm.ComputeHash(_bytes.ToArray()).ToHexString().ToLower();
		}
	}
}