using System;
using System.Collections.Generic;
using System.Linq;
using HelloApps.Services;
using Newtonsoft.Json;
using SYW.App.Pictures.Domain.Settings;
using log4net;

namespace SYW.App.Pictures.Domain.Services.Platform
{
    public interface IUserService
    {
        void Register();
        SocialUserInfo CurrentUser();
        List<SocialUserInfo> GetUsers(IList<long> ids);
        List<SocialUserInfo> GetUsersGetWithoutContext(IList<long> ids);
        void Follow(long followedUserId);
        void Unfollow(long followedUserId);
    	IList<long> GetFollowers(long userId);
    	IList<long> GetFollowedBy(long userId);
    }

    public class SocialUserInfo : IEquatable<SocialUserInfo>
    {
        public long id { get; set; }
        public string name { get; set; }
        public string sywrMemberNumber { get; set; }
        public string imageUrl { get; set; }
        public string profileUrl { get; set; }
        public string email { get; set; }
        public string imageUrlWithNoImageFallback { get { return !string.IsNullOrEmpty(imageUrl) ? imageUrl : "no-image-url-path"; } }


        public bool Equals(SocialUserInfo other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.id == id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(SocialUserInfo)) return false;
            return Equals((SocialUserInfo)obj);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public static bool operator ==(SocialUserInfo left, SocialUserInfo right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(SocialUserInfo left, SocialUserInfo right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("user {0} {1}", id, name);
        }
    }

    public class UserService : IUserService
    {
        private readonly IPlatformProxy _platformProxy;
        private readonly IApplicationSettings _applicationSettings;
        private readonly IAuthService _authService;
        private readonly IHashService _hashService;
        private readonly string _servicePath;
        private readonly ILog _logger;


        public UserService(IPlatformProxy platformProxy, IApplicationSettings applicationSettings, IAuthService authService, IHashService hashService, ILog logger)
        {
            _platformProxy = platformProxy;
            _applicationSettings = applicationSettings;
            _authService = authService;
            _hashService = hashService;
            _logger = logger;

            _servicePath = _applicationSettings.UsersPath;
        }

        public void Register()
        {
            _platformProxy.Get<string>(_servicePath + "/register-user");
        }

		public SocialUserInfo CurrentUser()
		{
            try
            {
                return _platformProxy.Get<SocialUserInfo>(_servicePath + "/current");
            } catch (Exception e)
            {
                _logger.Error("Could not get current user",e);
                return null;
            }
		}

    	public List<SocialUserInfo> GetUsers(IList<long> ids)
        {
            var requestParams = ids.Select(val => new KeyValuePair<string, object>("ids", val)).ToArray();
			//var requestParams = new[] {new KeyValuePair<string, object>("ids", ids)).ToArray()};

    		return _platformProxy.Get<List<SocialUserInfo>>(_servicePath + "/get", "POST", requestParams);
        }

        public List<SocialUserInfo> GetUsersGetWithoutContext(IList<long> ids)
        {
            var token = _authService.GetOfflineToken(2);
            var hash = _hashService.CreateSignature(token, _applicationSettings.AppSecret);
            var requestIds = ids.Select(val => new KeyValuePair<string, object>("ids", new long[] {val})).ToArray();
            var requestToken = new[]
			            {
			                new KeyValuePair<string, object>("token", token),
			                new KeyValuePair<string, object>("hash", hash)
			            };
            var requestParams = requestIds.Concat(requestToken).ToArray();

            var res = _platformProxy.GetWithoutContext<List<SocialUserInfo>>(_servicePath + "/get", requestParams);

            return res;
        }

        public void Follow(long followedUserId)
        {
            _platformProxy.Get<long>(_servicePath + "/follow",
                new KeyValuePair<string, object>("followedUserId", followedUserId));
        }

        public void Unfollow(long followedUserId)
        {
            _platformProxy.Get<long>(_servicePath + "/unfollow",
                new KeyValuePair<string, object>("followedUserId", followedUserId));
        }

		public IList<long> GetFollowedBy(long userId)
		{
			return _platformProxy.Get<long[]>(_servicePath + "/followers", new KeyValuePair<string, object>("userId", userId));
		}

		public IList<long> GetFollowers(long userId)
		{
			return _platformProxy.Get<long[]>(_servicePath + "/followed-by", new KeyValuePair<string, object>("userId", userId));
		}
    }
}