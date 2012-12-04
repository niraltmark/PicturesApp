using CommonGround.Settings;

namespace SYW.App.Messages.Web.Cookies
{
	public interface ICryptoSettings
	{
		[Default("c43dbae14eb0077fd7bab6a4804354abdd5c97852ec5d2141f49ca6fbc34c57f")]
		string EncryptionKey { get; set; }
	}
}