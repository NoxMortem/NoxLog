using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
#if ZENJECT
using Zenject;

#endif

namespace Infrastructure.NoxLog
{
	public class Logger
	{
		public static readonly Logger   StaticLogger = new Logger();
		private                string   filter       = "";
		private                LogLevel logLevel;

		public Logger(LogLevel logLevel = LogLevel.Trace)
		{
			SetLogLevel(logLevel);
		}

#if ZENJECT
		[Inject]
#endif
		public Logger(object context = null, LogLevel logLevel = LogLevel.Trace)
		{
			SetLogLevel(logLevel);
			SetContext(context);
		}

		public Logger() => SetLogLevel(LogLevel.Trace);

		public void SetContext(object context) => filter = GetFilter(context);

		public Logger For(object context)
		{
			SetContext(context);
			return this;
		}

		public Logger With(LogLevel level)
		{
			SetLogLevel(level);
			return this;
		}

		public void SetLogLevel(LogLevel level) => logLevel = level;

		public string GetFilter(object context)
		{
			switch (context)
			{
				case null:     return filter;
				case string s: return $"#{s}#";
				default:       return context is Type t ? $"#{t.Name}#" : $"#{context.GetType().Name}#";
			}
		}

		[Conditional("ENABLE_LOGGER"), DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void LogContext(string msg, object context = null)
		{
			var f = GetFilter(context);
			UnityEngine.Debug.Log(string.IsNullOrEmpty(f) ? $"{msg}" : $"{f}{msg}");
		}

		[Conditional("ENABLE_LOGGER"), DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void LogWarningContext(string msg, object context = null)
		{
			var f = GetFilter(context);
			UnityEngine.Debug.LogWarning(string.IsNullOrEmpty(f) ? $"{msg}" : $"{f}{msg}");
		}

		[Conditional("ENABLE_LOGGER"), DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void LogErrorContext(string msg, object context = null)
		{
			var f = GetFilter(context);
			UnityEngine.Debug.LogError(string.IsNullOrEmpty(f) ? $"{msg}" : $"{f}{msg}");
		}

		[Conditional("ENABLE_LOGGER"), DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Trace(string msg, object context = null)
		{
			if (logLevel <= LogLevel.Trace)
				LogContext(msg, context);
		}

		[Conditional("ENABLE_LOGGER"), DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Debug(string msg, object context = null)
		{
			if (logLevel <= LogLevel.Debug)
				LogContext(msg, context);
		}

		[Conditional("ENABLE_LOGGER"), DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Info(string msg, object context = null)
		{
			if (logLevel <= LogLevel.Info)
				LogContext(msg, context);
		}

		[Conditional("ENABLE_LOGGER"), DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Warning(string msg, object context = null)
		{
			if (logLevel <= LogLevel.Warning)
				LogWarningContext(msg, context);
		}

		[Conditional("ENABLE_LOGGER"), DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Error(string msg, object context = null)
		{
			if (logLevel <= LogLevel.Error)
				LogErrorContext(msg, context);
		}

		public static void LogTrace(string   msg)                 => StaticLogger.Trace(msg);
		public static void LogDebug(string   msg)                 => StaticLogger.Debug(msg);
		public static void LogInfo(string    msg)                 => StaticLogger.Info(msg);
		public static void LogWarning(string msg)                 => StaticLogger.Warning(msg);
		public static void LogError(string   msg)                 => StaticLogger.Error(msg);
		public static void LogTrace(object   context, string msg) => StaticLogger.Trace(msg, context);
		public static void LogDebug(object   context, string msg) => StaticLogger.Debug(msg, context);
		public static void LogInfo(object    context, string msg) => StaticLogger.Info(msg, context);
		public static void LogWarning(object context, string msg) => StaticLogger.Warning(msg, context);
		public static void LogError(object   context, string msg) => StaticLogger.Error(msg, context);

#if ZENJECT
		[UsedImplicitly]
		public class Factory : PlaceholderFactory<object, LogLevel, Logger> {}
#endif
	}
}