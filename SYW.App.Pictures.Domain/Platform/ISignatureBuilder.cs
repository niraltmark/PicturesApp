using System.Collections.Generic;

namespace SYW.App.Pictures.Domain.Services.Platform
{
	public interface ISignatureBuilder
	{
		ISignatureBuilder Append(IEnumerable<byte> bytes);
		string Sign();
	}
}