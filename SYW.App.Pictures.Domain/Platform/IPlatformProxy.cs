using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using PlatformClient.Platform;
using SYW.App.Pictures.Domain.WebClient;

namespace SYW.App.Pictures.Domain.Services.Platform
{
    public interface IPlatformProxy
    {
        TR Get<TR>(string servicePath, params KeyValuePair<string, object>[] parameters);
		TR Get<TR>(string servicePath, string method, params KeyValuePair<string, object>[] parameters);
    	TR GetWithoutContext<TR>(string servicePath, params KeyValuePair<string, object>[] parameters);
    	string GetOfflineToken(string servicePath, NameValueCollection parameters);
    	string Hash { get; }
    }

    public class PlatformProxy : IPlatformProxy
    {
        private readonly IWebClientBuilder _webClientBuilder;
        private readonly IPlatformConfiguration _platformConfiguration;
        private readonly IPlatformTokenProvider _platformTokenProvider;
        private readonly IPlatformHashProvider _platformHashProvider;
    	private readonly IPlatformParameterTranslator _platformParameterTranslator;

    	public PlatformProxy(IWebClientBuilder webClientBuilder, IPlatformConfiguration platformConfiguration, IPlatformTokenProvider platformTokenProvider, IPlatformHashProvider platformHashProvider,IPlatformParameterTranslator platformParameterTranslator)
        {
            _webClientBuilder = webClientBuilder;
            _platformConfiguration = platformConfiguration;
            _platformTokenProvider = platformTokenProvider;
            _platformHashProvider = platformHashProvider;
        	_platformParameterTranslator = platformParameterTranslator;
        }

        public TR Get<TR>(string servicePath, params KeyValuePair<string, object>[] parameters)
        {
        	return Get<TR>(servicePath, parameters, AddContextParameters);
        }

		public TR Get<TR>(string servicePath, string method, params KeyValuePair<string, object>[] parameters)
		{
			return Get<TR>(servicePath,  parameters, AddContextParameters, method);
		}

    	public TR GetWithoutContext<TR>(string servicePath, params KeyValuePair<string, object>[] parameters)
    	{
    		return Get<TR>(servicePath, parameters, null);
    	}

		public string GetOfflineToken(string servicePath, NameValueCollection serviceParameters)
    	{
			var webClient = _webClientBuilder.Create();

			var serviceUrl = GetSecureServiceUrl(servicePath);

			return webClient.GetJson<string>(serviceUrl, serviceParameters);
    	}

    	public string Hash
    	{
			get { return _platformHashProvider.GetHash(); }
    	}

    	private TR Get<TR>(string servicePath, ICollection<KeyValuePair<string, object>> parameters, Action<NameValueCollection> applyExtraParameters, string method = "GET")
    	{
    		var webClient = _webClientBuilder.Create();

    		var serviceUrl = GetServiceUrl(servicePath);

    		NameValueCollection serviceParameters;

    		if (parameters == null)
    		{
    			serviceParameters = new NameValueCollection(2);
    		}
    		else
    		{
    			serviceParameters = new NameValueCollection(parameters.Count + 2);

				foreach (var parameter in parameters)
				{
					var value = _platformParameterTranslator.TranslateParameters(parameter);
					
					serviceParameters.Add(parameter.Key,value);
				}
    			
    		}

			if (applyExtraParameters != null)
				applyExtraParameters(serviceParameters);

    		return webClient.GetJson<TR>(serviceUrl, serviceParameters, method);
    	}

    	private void AddContextParameters(NameValueCollection serviceParameters)
    	{
    		serviceParameters.Add("token", _platformTokenProvider.GetToken());
    		serviceParameters.Add("hash", _platformHashProvider.GetHash());
    	}

    	private Uri GetServiceUrl(string servicePath)
        {
            return new Uri(_platformConfiguration.PlatformApiBaseUrl, servicePath);
        }

		private Uri GetSecureServiceUrl(string servicePath)
		{
			return new Uri(_platformConfiguration.PlatformSecureApiBaseUrl, servicePath);
		}

		
    }
}