namespace SYW.App.Messages.Web.Cookies
{
	public interface ISerializer<TValue, TSerialized>
	{
		TSerialized Serialize(TValue value);
		TValue Deserialize(TSerialized serialized);
	}
}