using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using CommonGround;
using CommonGround.MvcInvocation;
using CommonGround.Settings;
using SYW.App.Pictures.Filters;

namespace SYW.App.Pictures.Config
{
	public class Bootstrapper
	{
		private readonly Assembly _currentAssembly;
		private readonly Assembly[] _assemblies;
		private static SettingsManager _settingsManager;

		private ICommonContainer _container;

		public Bootstrapper(ICommonContainer container)
		{
			_container = container;

			_currentAssembly = Assembly.GetExecutingAssembly();
			_assemblies = new[]
			{
			    _currentAssembly 
				//typeof (ILogger).Assembly,
				//typeof (Conversation).Assembly
			};
		}

		public void RegisterEverything()
		{
			//RegisterMongoDb();
			//InitializeLogger();
			RegisterDefaultInterfaces();
			RegisterSettingsManager();
			RegisterEvents();
			RegisterMvcApp();
			RegisterGlobalFilters();
		}

		public void RegisterDefaultInterfaces()
		{
			_container.AutoWire(_assemblies);
		}

		public void RegisterEvents()
		{
			_container.AutoWireEvents(_assemblies);
		}

		public void RegisterMvcApp()
		{
			_container.RegisterTypes(new Dictionary<Type, Type> { { typeof(IActionInvoker), typeof(PicturesActionInvoker) } });

			RoutesRegistrar.RegisterRoutes(RouteTable.Routes, new[] { _currentAssembly });

			var controllerTypes = RoutesRegistrar.GetControllerTypes(new[] { _currentAssembly }).ToArray();
			_container.RegisterTransients(controllerTypes);

			var controllerFactory = new WindsorControllerFactory(_container);
			ControllerBuilder.Current.SetControllerFactory(controllerFactory);
		}

		public void RegisterSettingsManager()
		{
			//var appSettings = _container.Resolve<IMongoStorage<SettingValue>>();
			//_settingsManager = new SettingsManager(_container, () => appSettings.GetAll().ToDictionary(x => x.Section + "." + x.Key, x => x.Value));
			//_settingsManager.RegisterSettings(new[] { _currentAssembly, typeof(Conversation).Assembly });
		}

		public void RegisterMongoDb()
		{
			//_container.RegisterTypes(new Dictionary<Type, Type> { { typeof(IMongoStorage<>), typeof(MongoStorage<>) } });

			//if (MongoAppHarborDatabaseProvider.IsViaAppHarbor)
			//{
			//    _container.RegisterInstance<IMongoDatabaseProvider>(new MongoAppHarborDatabaseProvider());
			//    return;
			//}

			//_container.RegisterInstance<IMongoServerProvider>(new MongoServerProvider());
			//_container.RegisterTypes(new Dictionary<Type, Type> { { typeof(IMongoDatabaseProvider), typeof(MongoDatabaseProvider) } });
		}

		public void InitializeLogger()
		{
			//var log = LogManager.GetLogger(typeof(MvcApplication));
			//XmlConfigurator.Configure();

			//_container.RegisterInstance(log);
		}

		public void RegisterGlobalFilters()
		{
			GlobalFilters.Filters.Add(new HandleErrorAttribute());
		}

		public static void RefreshSettings()
		{
			_settingsManager.Refresh();
		}
	}
}