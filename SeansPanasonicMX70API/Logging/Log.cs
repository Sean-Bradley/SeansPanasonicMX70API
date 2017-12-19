using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SeansPanasonicMX70API.Logging
{
    public class Log
    {
        public static void LogIt(LogType logType, String theText)
        {
            StreamWriter sw = null;
            FileStream fs = null;
            string sFileName = "";

            String foldername = DateTime.Now.Year.ToString("00") + DateTime.Now.Month.ToString("00") + DateTime.Now.Day.ToString("00");// ToShortDateString();
            if (!Directory.Exists("logs\\" + foldername))
            {
                Directory.CreateDirectory("logs\\" + foldername);
            }

            switch (logType)
            {
                case LogType.ApplicationError:
                    try
                    {
                        sFileName = "logs\\" + foldername + "\\ApplicationError_" + DateTime.Now.Hour.ToString("00") + ".log";
                        fs = File.Open(sFileName, FileMode.Append, FileAccess.Write);
                        sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                        sw.WriteLine(DateTime.Now.ToString() + " -> " + theText);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        //sFileName = "logs\\ApplicationErrorException_" + GameId.ToString() + ".log";
                        //fs = File.Open(sFileName, FileMode.Append, FileAccess.Write);
                        //sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                        //sw.WriteLine(DateTime.Now.ToString() + " -> " + theText + Environment.NewLine + e.Message);
                    }
                    finally
                    {
                        if (sw != null) { sw.Close(); }
                        if (fs != null) { fs.Close(); }
                    }
                    break;
                //case LogType.Spin:
                //    try
                //    {
                //        sFileName = "logs\\" + foldername + "\\Spin.log";
                //        fs = File.Open(sFileName, FileMode.Append, FileAccess.Write);
                //        sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                //        sw.WriteLine(DateTime.Now.ToString() + " -> " + theText);
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine(e.Message);
                //        //sFileName = "logs\\ApplicationErrorException_" + GameId.ToString() + ".log";
                //        //fs = File.Open(sFileName, FileMode.Append, FileAccess.Write);
                //        //sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
                //        //sw.WriteLine(DateTime.Now.ToString() + " -> " + theText + Environment.NewLine + e.Message);
                //    }
                //    finally
                //    {
                //        if (sw != null) { sw.Close(); }
                //        if (fs != null) { fs.Close(); }
                //    }
                //    break;
            }
        }
    }

    public enum LogType
    {
        ApplicationError
    }
}
