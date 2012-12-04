using System.Collections.Generic;
using HelloApps.Services;
using MongoDB.Bson;
using SYW.App.Pictures.Domain.DataAccess.Entities;
using SYW.App.Pictures.Domain.Settings;
using log4net;

namespace SYW.App.Pictures.Domain.Services.Platform
{
    public interface IAppLinksService
    {
        string Register(string title, string url);
        void Unregister();
        void RegisterForTag(long tagId, string title, string token);
    	string RegisterWithoutContext(ObjectId userId, string title, string url);
    	void UpdateNotifications(ObjectId userId, int amount);
    }

    public class AppLinksService : IAppLinksService
    {
        private readonly IPlatformProxy _platformProxy;
        private readonly IApplicationSettings _applicationSettings;
        private readonly string _servicePath;
        private readonly IHashService _hashService;
    	private readonly IEntitiesRepository _entitiesRepository;
    	private readonly ILog _log;

        public AppLinksService(IPlatformProxy platformProxy, IApplicationSettings applicationSettings, IHashService hashService, IEntitiesRepository entitiesRepository, ILog log)
        {
            _platformProxy = platformProxy;
            _applicationSettings = applicationSettings;
            _hashService = hashService;
        	_entitiesRepository = entitiesRepository;
        	_log = log;
        	_servicePath = _applicationSettings.AppLinkPath;
        }

        public string Register(string title, string url)
        {
            var requestParams = new[] 
                                {
                                    new KeyValuePair<string, object>("title", title),
                                    new KeyValuePair<string, object>("url", url)
                                };

            return _platformProxy.Get<string>(_servicePath + "/register", requestParams);
        }

		public void UpdateNotifications(ObjectId userId, int amount)
		{
			var token = GetOfflineToken(userId);

			if (string.IsNullOrEmpty(token))
			{
				_log.Warn("Couldn't update notifications for " + userId + ". The user has no offline token");
				return;
			}

			var hash = _hashService.CreateSignature(token, _applicationSettings.AppSecret);
			var requestParams = new[] {
				new KeyValuePair<string, object>("token", token),
				new KeyValuePair<string, object>("hash", hash),
				new KeyValuePair<string, object>("notificationsCount", amount)
			};

			_platformProxy.GetWithoutContext<string>(_servicePath + "/notifications/update-count", requestParams);
		}

		public string RegisterWithoutContext(ObjectId userId, string title, string url)
		{
			var token = GetOfflineToken(userId);

			if (string.IsNullOrEmpty(token))
			{
				_log.Warn("Couldn't register " + userId + " without context. The user has no offline tokern");
				return null;
			}

			var hash = _hashService.CreateSignature(token, _applicationSettings.AppSecret);
			var requestParams = new[]
			            {
			                new KeyValuePair<string, object>("token", token),
			                new KeyValuePair<string, object>("hash", hash),
							new KeyValuePair<string, object>("title", title),
                            new KeyValuePair<string, object>("url", url)
			            };

			return _platformProxy.GetWithoutContext<string>(_servicePath + "/register", requestParams);
		}

		private string GetOfflineToken(ObjectId userId)
		{
			var entity =  _entitiesRepository.Get(userId);

			if (entity == null)
				return null;

			return entity.PlatformOfflineToken;
		}

        public void Unregister()
        {
            _platformProxy.Get<string>(_servicePath + "/unregister");
        }

        public void RegisterForTag(long tagId, string title, string token)
        {
            var hash = _hashService.CreateSignature(token, _applicationSettings.AppSecret);
            var requestParams = new[] 
                                {
                                    new KeyValuePair<string, object>("tagId",  tagId.ToString()),
                                    new KeyValuePair<string, object>("title", title),
									new KeyValuePair<string, object>("token", token),
									new KeyValuePair<string, object>("hash", hash)
                                };

            _platformProxy.GetWithoutContext<string>(_servicePath + "/register/for-tag", requestParams);
        }
    }
}