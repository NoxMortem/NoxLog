#if LOGGER_EXTENSIONS
namespace Infrastructure.NoxLog
{
	public static class LoggerExtensions
	{
		public static void LogTrace(this   object context, string msg) => Logger.LogTrace(msg, context);
		public static void LogDebug(this   object context, string msg) => Logger.LogDebug(msg, context);
		public static void LogInfo(this    object context, string msg) => Logger.LogInfo(msg, context);
		public static void LogWarning(this object context, string msg) => Logger.LogWarning(msg, context);
		public static void LogError(this   object context, string msg) => Logger.LogError(msg, context);
	}
}
#endif