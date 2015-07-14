using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using UnityEngine;

namespace th.nx
{
    public static class Log
    {
        public static Level LOG_LEVEL = Level.DEBUG;

        public enum Level
        {
            DEBUG,
        }

        public static void debug(object msg)
        {
            //*
            if (LOG_LEVEL <= Level.DEBUG)
                printLog(msg, "D ", DateTime.Now);
            //*/
        }


        private static void printLog(object msg, string lv, DateTime datetime)
        {
            // "yyyy-MM-dd HH:mm:ss.ffffff [PID:TID] X {message}"
            StringBuilder sb = new StringBuilder();
            sb.Append(datetime.ToString("yyyy-MM-dd HH:mm:ss.fff ["));
            sb.Append(Process.GetCurrentProcess().Id);
            sb.Append(":");
            sb.Append(Thread.CurrentThread.ManagedThreadId);
            sb.Append("] ");
            sb.Append(lv);
            sb.Append(msg);

            UnityEngine.Debug.Log(sb);
        }
    }
}
