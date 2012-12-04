using System;
using SYW.App.Messages.Web.Cookies;

namespace SYW.App.Messages.Web.Services
{
    public class AuthenticationContext
    {
    	private readonly IStateProvider _stateProvider;

    	public AuthenticationContext(IStateProvider stateProvider, ICryptoService cryptoService)
		{
			_stateProvider = stateProvider;

			_stateProvider.CookieState("authctx", TimeSpan.FromMinutes(15), false, true)
    			.Signed(cryptoService, TimeSpan.Zero)
    			.Jsoned<EntityContext>();
		}
    }
}