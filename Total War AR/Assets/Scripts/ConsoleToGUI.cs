using UnityEngine;

namespace DebugStuff {
    public class ConsoleToGUI : MonoBehaviour {
        //#if !UNITY_EDITOR
        static string myLog = "";
        private string output;
        private string stack;

        void OnEnable() {
            Application.logMessageReceived += Log;
        }

        void OnDisable() {
            Application.logMessageReceived -= Log;
        }

        public void Log(string logString, string stackTrace, LogType type) {
            output = logString;
            stack = stackTrace;
            if (stack.Length > 0)
                myLog = output + "\n" + stack + "\n" + myLog;
            else
                myLog = output + "\n" + myLog;
            if (myLog.Length > 5000) {
                myLog = myLog.Substring(0, 4000);
            }
        }

        void OnGUI() {
            myLog = GUI.TextArea(new Rect(50, 50, Screen.width - 100, Screen.height / 2f - 50), myLog);
        }
        //#endif
    }
}