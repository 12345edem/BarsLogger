using System;
using System.IO;
using System.Collections.Generic;

namespace BarsLogger
{
    //Модуль для записи информации непосредственно в файлы
    public class FileWriter
    {
        public static bool isConsoleEnabled = true;
        public static void Write(string writePath, string logName, string message)
        {
            try
            {
                using(StreamWriter sw = new StreamWriter(writePath,true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(logName +": " + message);
                }
                if (isConsoleEnabled)
                    Console.WriteLine(logName + " added to log " + writePath);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public static void Write(string writePath, string logName, string message, Exception e)
        {
            try
            {
                using(StreamWriter sw = new StreamWriter(writePath,true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(logName + ": " + message + "; StackTrace: " + e);
                }
                if (isConsoleEnabled)
                    Console.WriteLine(logName + " added to log " + writePath);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public static void Write(string writePath, string logName, string message, params object[] args)
        {
            try
            {
                var argv = "";
                for(int i = 0; i < args.Length; i++)
                {
                    argv += args[i];
                }
                using(StreamWriter sw = new StreamWriter(writePath,true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(logName + ": " + message + "; Args: " + argv);
                }
                if (isConsoleEnabled)
                    Console.WriteLine(logName + " added to log " + writePath);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public static void Write(string writePath, string logName, Exception e)
        {
            try
            {
                using(StreamWriter sw = new StreamWriter(writePath,true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(logName + "; StackTrace: " + e);
                }
                if (isConsoleEnabled)
                    Console.WriteLine(logName + " added to log " + writePath);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
        public static void Write(string writePath, string logName, string message, Dictionary<object, object> properties = null)
        {
            try
            {
                using(StreamWriter sw = new StreamWriter(writePath,true, System.Text.Encoding.Default))
                {
                    sw.WriteLine(logName + ". New Dictionary: ");
                    foreach(KeyValuePair<object, object> p in properties)
                    {
                        sw.WriteLine("{0}: {1}", p.Key.ToString(), p.Value.ToString());
                    }
                    sw.WriteLine("End of dictionary.");
                    sw.WriteLine();
                }
                if (isConsoleEnabled)
                    Console.WriteLine(logName + " added to log " + writePath);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}