using System;
using CommonGround.Events;
using log4net;
using log4net.Core;

namespace SYW.App.Pictures.Domain.Services.Logging
{
	public class EventLogger : IEventLogger
	{
		private readonly ILog _log;

		public EventLogger(ILog log)
		{
			_log = log;
		}

		public void Log(Delegate subscriber, Type type, Exception exception)
		{
			_log.Error("Error invoking " + subscriber + " for event " + type, exception);
		}
	}
}