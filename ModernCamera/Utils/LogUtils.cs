using System;
using BepInEx.Logging;

namespace ModernCamera.Utils
{
    public class LogUtils
    {
        private static ManualLogSource Log;

        public static void LogDebugError(Exception exception)
        {
            Log?.LogError(exception);
        }

        public static void Init(ManualLogSource log)
        {
            Log = log;
        }

        public static void LogInfo(string text)
        {
            Log?.LogInfo(text);
        }

        public static void LogWarning(string s)
        {
            Log?.LogWarning(s);
        }
    }
}
