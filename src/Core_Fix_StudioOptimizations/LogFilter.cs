using BepInEx;
using HarmonyLib;
using Studio;
using UnityEngine;

namespace IllusionFixes
{
    /// <summary>
    /// Filtering logs with lots of output.
    /// </summary>
    public partial class StudioOptimizations : BaseUnityPlugin
    {
        static string[] _filterTargets = new string[]
        {
            "Look rotation viewing vector is zero",
        };

        static int _beforeMatchIndex = -1;

        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(BepInEx.Logging.UnityLogSource), "UnityLogMessageHandler")]
        private static bool UnityLogMessageHandlerPrefix(object sender, BepInEx.Logging.LogEventArgs eventArgs)
        //[HarmonyPatch(typeof(BepInEx.Logging.UnityLogSource), "OnUnityLogMessageReceived")]
        //private static bool OnUnityLogMessageReceivedPrefix(string message, string stackTrace, LogType type)
        {
            System.Console.WriteLine($"[[{eventArgs?.Data?.ToString()}]]");
            return false;

            if (eventArgs == null)
                return false;

            if (sender == null)
                return false;

            string message = eventArgs?.Data.ToString();
            //sender = message;
            
        }

        static public bool LogFilterHander( object message )
        {
            System.Console.WriteLine($"@@ {message}");
            if( message is string )
                return LogFilter((string)message);

            return true;
        }

        static public bool LogFilterHander(object message, UnityEngine.Object context)
        {
            System.Console.WriteLine($"@@@ {message}");

            if (message is string)
                return LogFilter((string)message);
            return false;
        }

        static bool LogFilter( string message )
        {
            for (int i = 0; i < _filterTargets.Length; ++i)
            {
                if (message.StartsWith(_filterTargets[i]))
                {
                    if (_beforeMatchIndex == i)
                    {
                        throw null;
                        return false;
                    }
                        

                    // Only one output.
                    _beforeMatchIndex = i;
                    return true;
                }
            }

            _beforeMatchIndex = -1;
            return true;
        }
    }
}
