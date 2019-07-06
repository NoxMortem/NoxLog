#if ZENJECT && LOGGER_EXTENSIONS
using Zenject;

namespace Infrastructure.NoxLog.ZenjectExtensions
{
	public static class ZenjectExtensions
	{
		public static void BindLogger(this DiContainer container, LogLevel logLevel)
		{
			container.Bind<Logger>().AsTransient().WithArguments(typeof(Logger), logLevel);
			Logger.StaticLogger.SetLevel(logLevel);
		}

		public static void BindLogger<T>(this DiContainer container, LogLevel logLevel)
		{
			container.Bind<Logger>().AsTransient().WithArguments(typeof(T), logLevel).WhenInjectedInto<T>();
		}
	}
}
#endif