using System.Web.Mvc;
using CommonGround.MvcInvocation;

namespace SYW.App.Pictures.Filters
{
	public class PicturesActionInvoker : ActionInvoker
	{
		//private readonly ITokenExtractorFilter _tokenExtractorFilter;
		//private readonly IAutoLoginFilter _autoLoginFilter;
		//private readonly IOfflineTokenProviderFilter _offlineTokenProviderFilter;
		//private readonly IProfilerFilter _profilerFilter;
		//private readonly IExceptionFilter _exceptionFilter;

		public PicturesActionInvoker()
		{
			//_tokenExtractorFilter = tokenExtractorFilter;
			//_autoLoginFilter = autoLoginFilter;
			//_offlineTokenProviderFilter = offlineTokenProviderFilter;
			//_profilerFilter = profilerFilter;
			//_exceptionFilter = exceptionFilter;
		}

		protected override FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
		{
			var filterInfo = base.GetFilters(controllerContext, actionDescriptor);

			//filterInfo.AuthorizationFilters.Add(_tokenExtractorFilter);
			//filterInfo.AuthorizationFilters.Add(_autoLoginFilter);
			//filterInfo.AuthorizationFilters.Add(_offlineTokenProviderFilter);
			//filterInfo.AuthorizationFilters.Add(_profilerFilter);

			//filterInfo.ExceptionFilters.Add(_exceptionFilter);

			return filterInfo;
		}
	}
}