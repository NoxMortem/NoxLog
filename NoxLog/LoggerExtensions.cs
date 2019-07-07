#if LOGGER_EXTENSIONS
namespace Infrastructure.NoxLog
{
	public static class LoggerExtensions
	{
		public static void LogTrace(this   object context, string msg) => Logger.StaticLogger.Trace(msg, context);
		public static void LogDebug(this   object context, string msg) => Logger.StaticLogger.Debug(msg, context);
		public static void LogInfo(this    object context, string msg) => Logger.StaticLogger.Info(msg, context);
		public static void LogWarning(this object context, string msg) => Logger.StaticLogger.Warning(msg, context);
		public static void LogError(this   object context, string msg) => Logger.StaticLogger.Error(msg, context);
	}
}
#endif