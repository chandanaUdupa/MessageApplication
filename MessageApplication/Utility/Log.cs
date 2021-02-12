using System.Diagnostics;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace StandardModel.Diagnostic
{
    public interface ILoggable
    {
        DiagnosticLog.ApplicationModules MyModule { get; }
        string GetCurrentUser { get; }
        DiagnosticLog.LogType LogType { get; }
    }

    public static class LogExtension
    {

        public static void PublicMessageBegin(this ILogger log, string message, [CallerMemberName] string caller = "Unknow caller")
        {
            string header = $"-------- { caller }--------";
            log.PublicMessageSingleLogInfo(header);
            log.PublicMessageSingleLogInfo(message);
        }

        public static void PublicMessageEnd(this ILogger log, string message, [CallerMemberName] string caller = "Unknow caller")
        {
            string footer = $"-------- { caller }--------";
            log.PublicMessageSingleLogInfo(message);
            log.PublicMessageSingleLogInfo(footer);
        }



        public static void PublicMessageSingleLogInfo(this ILogger log, string message)
        {
            log.LogInformation("");
            log.LogInformation("-- PUBLIC MESSAGE -- " + message);
            log.LogInformation("");
        }

        public static void PublicMessageSingleLogError(this ILogger log, string message)
        {
            log.LogError("");
            log.LogError("-- PUBLIC MESSAGE -- " + message);
            log.LogError("");
        }

        public static void PublicMessageSingleLogWarning(this ILogger log, string message)
        {
            log.LogWarning("");
            log.LogWarning("-- PUBLIC MESSAGE -- " + message);
            log.LogWarning("");
        }
    }

    public static class DiagnosticLog
    {
        public enum ApplicationModules : long
        {
            Writeback = 0b1_0000_0000_0000_0000_0000_0000_0000_0001
        }

        public enum LogType
        {
            Debug,
            Performance,
            Both
        }

        private static long appToTrace;


        public static void TraceModule(ApplicationModules module)
        {
            appToTrace |= (long)module;
        }

        public static void TraceModule(long modules)
        {
            appToTrace = modules;
        }

        public static void Debug(string message, ILoggable loggable)
        {
            if ((appToTrace & (long)(loggable.MyModule)) != 0L)
            {
                if (loggable.LogType == LogType.Debug || loggable.LogType == LogType.Both)
                {
                    Trace.Write($"User: {loggable.GetCurrentUser}" + message);
                }
            }
        }

        public static void Performance(string message, ILoggable loggable)
        {
            if ((appToTrace & (long)(loggable.MyModule)) != 0L)
            {
                if (loggable.LogType == LogType.Performance || loggable.LogType == LogType.Both)
                {
                    Trace.Write($"User: {loggable.GetCurrentUser}" + message);
                }
            }
        }
    }
}
