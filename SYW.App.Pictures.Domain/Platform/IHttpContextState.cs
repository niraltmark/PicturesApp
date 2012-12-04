using System.Web;

namespace PlatformClient.Platform
{
    public interface IHttpContextState
    {
        T GetItem<T>(string key);
        void SetItem<T>(string key, T value);
    }

    public class HttpContextState : IHttpContextState
    {
        public T GetItem<T>(string key)
        {
            return (T)HttpContext.Current.Items[key];
        }

        public void SetItem<T>(string key, T value)
        {
            HttpContext.Current.Items[key] = value;
        }
    }
}