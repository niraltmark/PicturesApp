using System.Web.Mvc;
using CommonGround.MvcInvocation;
using SYW.App.Pictures.Web.Filters;

namespace SYW.App.Pictures.Filters
{
	public class PicturesActionInvoker : ActionInvoker
	{
		private readonly ITokenExtractorFilter _tokenExtractorFilter;
		private readonly IAutoLoginFilter _autoLoginFilter;
		private readonly IOfflineTokenProviderFilter _offlineTokenProviderFilter;
		private readonly IExceptionFilter _exceptionFilter;

		public PicturesActionInvoker(ITokenExtractorFilter tokenExtractorFilter, IAutoLoginFilter autoLoginFilter, IOfflineTokenProviderFilter offlineTokenProviderFilter, IExceptionFilter exceptionFilter)
		{
			_tokenExtractorFilter = tokenExtractorFilter;
			_autoLoginFilter = autoLoginFilter;
			_offlineTokenProviderFilter = offlineTokenProviderFilter;
			_exceptionFilter = exceptionFilter;
		}

		protected override FilterInfo GetFilters(ControllerContext controllerContext, ActionDescriptor actionDescriptor)
		{
			var filterInfo = base.GetFilters(controllerContext, actionDescriptor);

			filterInfo.AuthorizationFilters.Add(_tokenExtractorFilter);
			filterInfo.AuthorizationFilters.Add(_autoLoginFilter);
			filterInfo.AuthorizationFilters.Add(_offlineTokenProviderFilter);

			filterInfo.ExceptionFilters.Add(_exceptionFilter);

			return filterInfo;
		}
	}
}