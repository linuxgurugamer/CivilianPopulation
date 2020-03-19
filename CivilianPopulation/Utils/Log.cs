using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*
MIT License

Copyright (c) 2019 Heiswayi Nrird

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/


[KSPAddon(KSPAddon.Startup.MainMenu, true)]
internal class RootPath : MonoBehaviour
{
    static internal string rootPath;
    void Start()
    {
        rootPath = KSPUtil.ApplicationRootPath;
        Log.InitLog(rootPath);
    }
}
static public class Log
{
    public enum LEVEL
    {
        OFF,
        INFO,
        DETAIL,
        WARNING,
        DEBUG,
        ERROR,
        TRACE,
        FATAL
    };

    static private string TITLE = "CivilianPopulation";
    static public LEVEL level = LEVEL.INFO;

    static private String PREFIX;

    private const string FILE_EXT = ".log";
    static private string datetimeFormat;
    static private string logFilename;
    static bool initted = false;

    static internal bool SeperateLogfile { get; set; }
    static internal bool StandardLog { get; set; }

    /// <summary>
    /// Initiate an instance of SimpleLogger class constructor.
    /// If log file does not exist, it will be created automatically.
    /// </summary>
    static internal void InitLog(string rootPath)
    {
        PREFIX = TITLE + ": ";
        datetimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        string PATH = KSPUtil.ApplicationRootPath + "logs";

        if (!Directory.Exists(PATH))
        {
            UnityEngine.Debug.Log("Creating Logs directory: " + PATH);
            Directory.CreateDirectory(PATH);
        }
        logFilename = PATH + "/" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + FILE_EXT;

        // Log file header line
        string logHeader = logFilename + " is created.";
        if (!System.IO.File.Exists(logFilename))
        {
            WriteLine(System.DateTime.Now.ToString(datetimeFormat) + " " + logHeader, false);
        }
        initted = true;
        SeperateLogfile = true;
        StandardLog = true;
    }

    static public void SetTitle(string str)
    {
        TITLE = str;
        PREFIX = TITLE + ": ";
    }

    static public LEVEL GetLevel()
    {
        return level;
    }

    static public void SetLevel(LEVEL l)
    {
        UnityEngine.Debug.Log("log level " + l);
        level = l;
    }

    static public LEVEL GetLogLevel()
    {
        return level;
    }

    static private bool IsLevel(LEVEL l)
    {
        return level == l;
    }

    static public bool IsLogable(LEVEL l)
    {
        return level <= l;
    }

    /// <summary>
    /// Log a TRACE message
    /// </summary>
    /// <param name="text">Message</param>
    static public void Trace(String msg)
    {
        if (IsLogable(LEVEL.TRACE))
        {
            WriteFormattedLog(LEVEL.TRACE, msg);
            //UnityEngine.Debug.Log(PREFIX + msg);
        }
    }

    /// <summary>
    /// Log an INFO message
    /// </summary>
    /// <param name="text">Message</param>
    [ConditionalAttribute("DEBUG")]
    static public void Info(String msg)
    {
        if (IsLogable(LEVEL.INFO))
        {
            WriteFormattedLog(LEVEL.INFO, msg);
            //UnityEngine.Debug.Log(PREFIX + msg);
        }
    }

    static public void Detail(String msg)
    {
        if (IsLogable(LEVEL.DETAIL))
        {
            WriteFormattedLog(LEVEL.DETAIL, msg);
            //UnityEngine.Debug.Log(PREFIX + msg);
        }
    }


    [ConditionalAttribute("DEBUG")]
    static public void Test(String msg)
    {
        //if (IsLogable(LEVEL.INFO))
        {
            WriteFormattedLog(LEVEL.DEBUG, msg);
            //UnityEngine.Debug.LogWarning(PREFIX + "TEST:" + msg);
        }
    }


    /// <summary>
    /// Log a DEBUG message
    /// </summary>
    /// <param name="text">Message</param>
    static public void Debug(string text)
    {
        WriteFormattedLog(LEVEL.DEBUG, text);
    }

    /// <summary>
    /// Log an ERROR message
    /// </summary>
    /// <param name="text">Message</param>
    static public void Error(string text)
    {
        WriteFormattedLog(LEVEL.ERROR, text);
    }

    /// <summary>
    /// Log a FATAL ERROR message
    /// </summary>
    /// <param name="text">Message</param>
    static public void Fatal(string text)
    {
        WriteFormattedLog(LEVEL.FATAL, text);
    }



    /// <summary>
    /// Log a WARNING message
    /// </summary>
    /// <param name="text">Message</param>
    static public void Warning(string text)
    {
        WriteFormattedLog(LEVEL.WARNING, text);
    }

    static private void WriteLine(string text, bool append = true)
    {
        try
        {
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(logFilename, append, System.Text.Encoding.UTF8))
            {
                if (!string.IsNullOrEmpty(text))
                {
                    writer.WriteLine(text);
                }
            }
        }
        catch
        {
            throw;
        }
    }

    static private void WriteFormattedLog(LEVEL level, string text)
    {
        string pretext;
        switch (level)
        {
            case LEVEL.OFF:
                return;
            case LEVEL.INFO:
                pretext = System.DateTime.Now.ToString(datetimeFormat) + " [INFO]    ";
                break;
            case LEVEL.DETAIL:
                pretext = System.DateTime.Now.ToString(datetimeFormat) + " [DETAIL]   ";
                break;
            case LEVEL.WARNING:
                pretext = System.DateTime.Now.ToString(datetimeFormat) + " [WARNING] ";
                break;
            case LEVEL.DEBUG:
                pretext = System.DateTime.Now.ToString(datetimeFormat) + " [DEBUG]   ";
                break;
            case LEVEL.ERROR:
                pretext = System.DateTime.Now.ToString(datetimeFormat) + " [ERROR]   ";
                break;
            case LEVEL.TRACE:
                pretext = System.DateTime.Now.ToString(datetimeFormat) + " [TRACE]   ";
                break;
            case LEVEL.FATAL:
                pretext = System.DateTime.Now.ToString(datetimeFormat) + " [FATAL]   ";
                break;
            default:
                pretext = "";
                break;
        }
        if (initted)
        {
            if (SeperateLogfile)
                WriteLine(pretext + text);
            if (StandardLog)
                UnityEngine.Debug.Log(pretext +  PREFIX + text);
        }
    }
}

