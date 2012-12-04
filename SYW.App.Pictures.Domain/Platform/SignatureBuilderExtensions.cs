using System;
using System.Text;

namespace SYW.App.Pictures.Domain.Services.Platform
{
	public static class SignatureBuilderExtensions
	{
		public static ISignatureBuilder Append(this ISignatureBuilder signatureBuilder, long value)
		{
			return signatureBuilder.Append(BitConverter.GetBytes(value));
		}

		public static ISignatureBuilder Append(this ISignatureBuilder signatureBuilder, int value)
		{
			return signatureBuilder.Append(BitConverter.GetBytes(value));
		}

		public static ISignatureBuilder Append(this ISignatureBuilder signatureBuilder, DateTime value)
		{
			return signatureBuilder.Append(BitConverter.GetBytes(value.ToUnixTime()));
		}

		public static ISignatureBuilder Append(this ISignatureBuilder signatureBuilder, string value)
		{
			return signatureBuilder.Append(Encoding.UTF8.GetBytes(value));
		}

		public static ISignatureBuilder Append(this ISignatureBuilder signatureBuilder, Guid value)
		{
			return signatureBuilder.Append(value.ToByteArray());
		}
	}
}
