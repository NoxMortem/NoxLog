using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
#if ZENJECT
using Zenject;

#endif

namespace Infrastructure.NoxLog
{
	public class Logger
	{
		public static readonly Logger StaticLogger = new Logger();
		public static readonly Logger None         = new Logger(LogLevel.None);

		private string   filter = "";
		private LogLevel LogLevel { get; set; }

		private static readonly Dictionary<Type, LogLevel> LogLevels = new Dictionary<Type, LogLevel>();

		public Logger(LogLevel LogLevel = LogLevel.Trace)
		{
			SetLogLevel(LogLevel);
		}

#if ZENJECT
		[Inject]
#endif
		public Logger(object context = null, LogLevel LogLevel = LogLevel.Trace)
		{
			SetLogLevel(LogLevel);
			SetContext(context);
		}

		public Logger() => SetLogLevel(LogLevel.Trace);

		private void SetContext(object context) => filter = $"#{GetContextName(context)}#";

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

		public void SetLogLevel(LogLevel level) => LogLevel = level;

		public static void Configure<T>(LogLevel level) => LogLevels[typeof(T)] = level;

		public static void Configure(Dictionary<Type, LogLevel> configuration)
		{
			foreach (var kvp in configuration)
				LogLevels[kvp.Key] = kvp.Value;
		}

		private string GetContextName(object context)
		{
			switch (context)
			{
				case null:     return "";
				case string s: return $"{s}";
				case int i:    return $"{i.ToString()}";
				case double d: return $"{d.ToString(CultureInfo.InvariantCulture)}";
				case float f:  return $"{f.ToString(CultureInfo.InvariantCulture)}";
				default:       return context is Type t ? $"{t.Name}" : $"{context.GetType().Name}";
			}
		}

		[Conditional("ENABLE_LOGGER"), DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void LogContext(string msg, object context = null)
		{
			var f = GetContextName(context);
			UnityEngine.Debug.Log(string.IsNullOrEmpty(f) ? $"{msg}" : $"#{f}#{msg}");
		}

		[Conditional("ENABLE_LOGGER"), DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void LogWarningContext(string msg, object context = null)
		{
			var f = GetContextName(context);
			UnityEngine.Debug.LogWarning(string.IsNullOrEmpty(f) ? $"{msg}" : $"{f}:{msg}");
		}

		[Conditional("ENABLE_LOGGER"), DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void LogErrorContext(string msg, object context = null)
		{
			var f = GetContextName(context);
			UnityEngine.Debug.LogError(string.IsNullOrEmpty(f) ? $"{msg}" : $"{f}:{msg}");
		}

		[Conditional("ENABLE_LOGGER"), DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Trace(string msg, object context = null)
		{
			if (LogLevel <= LogLevel.Trace)
				LogContext(msg, context);
		}

		[Conditional("ENABLE_LOGGER"), DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Debug(string msg, object context = null)
		{
			if (LogLevel <= LogLevel.Debug)
				LogContext(msg, context);
		}

		[Conditional("ENABLE_LOGGER"), DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Info(string msg, object context = null)
		{
			if (LogLevel <= LogLevel.Info)
				LogContext(msg, context);
		}

		[Conditional("ENABLE_LOGGER"), DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Warning(string msg, object context = null)
		{
			if (LogLevel <= LogLevel.Warning)
				LogWarningContext(msg, context);
		}

		[Conditional("ENABLE_LOGGER"), DebuggerHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Error(string msg, object context = null)
		{
			if (LogLevel <= LogLevel.Error)
				LogErrorContext(msg, context);
		}

		public static void LogTrace(string msg, object context = null)
		{
			var level = StaticLogger.LogLevel;
			if (context != null && LogLevels.TryGetValue(context is Type t ? t : context.GetType(), out var ll))
				level = ll;

			if (level <= LogLevel.Trace)
				StaticLogger.LogContext(msg, context);
		}

		public static void LogDebug(string msg, object context = null)
		{
			var level = StaticLogger.LogLevel;
			if (context != null && LogLevels.TryGetValue(context is Type t ? t : context.GetType(), out var ll))
				level = ll;

			if (level <= LogLevel.Debug)
				StaticLogger.LogContext(msg, context);
		}

		public static void LogInfo(string msg, object context = null)
		{
			var level = StaticLogger.LogLevel;
			if (context != null && LogLevels.TryGetValue(context is Type t ? t : context.GetType(), out var ll))
				level = ll;

			if (level <= LogLevel.Info)
				StaticLogger.LogContext(msg, context);
		}

		public static void LogWarning(string msg, object context = null)
		{
			var level = StaticLogger.LogLevel;
			if (context != null && LogLevels.TryGetValue(context is Type t ? t : context.GetType(), out var ll))
				level = ll;

			if (level <= LogLevel.Warning)
				StaticLogger.LogWarningContext(msg, context);
		}

		public static void LogError(string msg, object context = null)
		{
			var level = StaticLogger.LogLevel;
			if (context != null && LogLevels.TryGetValue(context is Type t ? t : context.GetType(), out var ll))
				level = ll;

			if (level <= LogLevel.Error)
				StaticLogger.LogErrorContext(msg, context);
		}

#if ZENJECT
		[UsedImplicitly]
		public class Factory : PlaceholderFactory<object, LogLevel, Logger> {}
#endif
	}
}