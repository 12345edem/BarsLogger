using System;
using System.IO;
using System.Collections.Generic;

namespace Test
{
    
    /// <summary>Интерфейс работы с логом</summary>
    public interface ILog
    {
        /// <summary>
        /// Критичная ошибка:приложение не может далее функционировать
        /// </summary>
        /// <param name="message">сообщение</param>
        void Fatal(string message);

        /// <summary>
        /// Критичная ошибка:приложение не может далее функционировать
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <param name="e">Exception</param>
        void Fatal(string message, Exception e);

        /// <summary>
        /// Ошибка в работе приложения: операция расчета завершается, но приложение продолжает работу
        /// </summary>
        /// <param name="message">сообщение</param>
        void Error(string message);

        /// <summary>
        /// Ошибка в работе приложения: операция расчета завершается, но приложение продолжает работу
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <param name="e">Exception</param>
        void Error(string message, Exception e);

        /// <summary>
        /// Ошибка в работе приложения: операция расчета завершается, но приложение продолжает работу
        /// </summary>
        /// <param name="ex"></param>
        void Error(Exception ex);

        /// <summary>
        /// Запись уникальных ошибок
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        void ErrorUnique(string message, Exception e);

        /// <summary>
        /// Предупреждение: на работу приложения не влияет, 
        /// но может сообщать о потенциальных проблемах в расчете
        /// </summary>
        /// <param name="message">сообщение</param>
        void Warning(string message);

        /// <summary>
        /// Предупреждение: на работу приложения не влияет, 
        /// но может сообщать о потенциальных проблемах в расчете
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <param name="e">Exception</param>
        void Warning(string message, Exception e);


        /// <summary>
        /// Пишет в лог уникальные в течении дня ошибки 
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <remarks>
        /// Если в течении дня поступают сообщения с одинаковым содержанием,
        ///  то в лог попадут только первые вхождения. 
        /// По прошествию дня уникальность возобновляется.
        /// </remarks>>
        void WarningUnique(string message);

        /// <summary>
        /// Информирование: не влияет на работу приложения,
        /// является инструментом информирования
        /// </summary>
        /// <param name="message">сообщение</param>
        void Info(string message);

        /// <summary>
        /// Информирование: не влияет на работу приложения,
        /// является инструментом информирования
        /// </summary>
        /// <param name="message">сообщение</param>
        ///  /// <param name="e">Exception</param>
        void Info(string message, Exception e);

        /// <summary>
        /// Информирование: не влияет на работу приложения,
        /// является инструментом информирования
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <param name="args">аргументы</param>
        void Info(string message, params object[] args);

        /// <summary>
        /// Дебагирование: инструмент для трассировки и отладки
        /// </summary>
        /// <param name="message">сообщение</param>
        void Debug(string message);

        /// <summary>
        /// Дебагирование: инструмент для трассировки и отладки
        /// </summary>
        /// <param name="message">сообщение</param>
        /// <param name="e">Exception</param>
        void Debug(string message, Exception e);

        /// <summary>
        /// Дебагирование: инструмент для трассировки и отладки
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args">аргументы</param>
        void DebugFormat(string message, params object[] args);

        /// <summary>
        /// Запись системных логов информационного характера
        /// </summary>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        void SystemInfo(string message, Dictionary<object, object> properties = null);
    }

    class Logger : ILog 
    {
        //Расширение файла
        private const string fileExt = ".log";
        //Переменные пути сохранения
        private static string dirPath;
        private string fatalErrorPath;
        private string errorPath;
        private string errorUniquePath;
        private string warningPath;
        private string warningUniquePath;
        private string infoPath;
        private string debugPath;
        private string systemInfoPath;
        //Имена логов
        private const string fatalName = "Fatal";
        private const string errorName = "Error";
        private const string errorUniqueName = "ErrorUnique";
        private const string warningName = "Warning";
        private const string warningUniqueName = "Warning Unique";
        private const string infoName = "Info";
        private const string debugName = "Debug";
        private const string systemInfoName = "SystemInfo";
        //Массивы для проверки уникальных ошибок и ворнингов
        private static List<string> errorUniqueListMessage = new List<string>();
        public static List<string> warningUniqueListMessage = new List<string>();
        //Включение вывода в консоль о записанных файлах
        public static bool isConsoleEnabled = true;
        //Создаем новую папку каждый день, очищаем память уникальных списков
        public Logger()
        {
            dirPath = @"Logger\" + DateTime.Now.Date.ToString("d");
            MainLoop();
            Paths();
            Init();
        }
        public void MainLoop()
        {
            if(!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                Init();
            }
        }
        private void Init()
        {
            errorUniqueListMessage.Clear();
            warningUniqueListMessage.Clear();
            try
            {
                //Учет уникальных ворнингов
                using(StreamReader sr = new StreamReader(warningUniquePath))
                    while (!sr.EndOfStream)
                        warningUniqueListMessage.Add(sr.ReadLine());
            }
            catch(Exception ex)
            {
                ex.ToString();//Console.WriteLine(ex);
            }
            
            //Учет остальных ворнингов
            try
            {   
                using(StreamReader sr = new StreamReader(warningPath))
                    while(!sr.EndOfStream)
                        warningUniqueListMessage.Add(sr.ReadLine());       
            }
            catch(Exception ex)
            {
                ex.ToString();//Console.WriteLine(ex);
            }

            //Учет уникальных ошибок
            try
            {
                using(StreamReader sr = new StreamReader(errorUniquePath))
                    while(!sr.EndOfStream)
                        errorUniqueListMessage.Add(sr.ReadLine());
            }
            catch(Exception ex)
            {
                ex.ToString();//Console.WriteLine(ex);
            }
            //Учет остальных ошибок
            try
            {
                using(StreamReader sr = new StreamReader(errorPath))
                    while(!sr.EndOfStream)
                        errorUniqueListMessage.Add(sr.ReadLine());
            }
            catch(Exception ex)
            {
                ex.ToString();//Console.WriteLine(ex);
            }
            
        }
        //Пути сохранения по определенным видам логов
        private void Paths()
        {
            fatalErrorPath = dirPath + @"\Fatal" + fileExt;
            errorPath = dirPath + @"\Error" + fileExt;
            errorUniquePath = dirPath + @"\ErrorUnique" + fileExt;
            warningPath = dirPath + @"\Warning" + fileExt;
            warningUniquePath = dirPath + @"\WarningUnique" + fileExt;
            infoPath = dirPath + @"\Info" + fileExt;
            debugPath = dirPath + @"\Debug" + fileExt;
            systemInfoPath = dirPath + @"\SystemInfo" + fileExt;
        }
        //Методы записи в файлы(OVVVERLADING)
        static void Write(string writePath, string logName, string message)
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
        static void Write(string writePath, string logName, string message, Exception e)
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
        static void Write(string writePath, string logName, string message, params object[] args)
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
        static void Write(string writePath, string logName, Exception e)
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
        static void Write(string writePath, string logName, string message, Dictionary<object, object> properties = null)
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

        //Реализация методов интерфейса ILog
        public void Fatal(string message)
        {
            Write(fatalErrorPath, fatalName, message);
        }
        public void Fatal(string message, Exception e)
        {
            Write(fatalErrorPath, fatalName, message, e);
        }
        public void Error(string message)
        {
            Write(errorPath, errorName, message);
            errorUniqueListMessage.Add(message);
        }
        public void Error(Exception e)
        {
            Write(errorPath, errorName, e);
            errorUniqueListMessage.Add(e.ToString());
        }
        public void Error(string message, Exception e)
        {
            Write(errorPath, errorName, message, e);
            errorUniqueListMessage.Add(message);
            errorUniqueListMessage.Add(e.ToString());
        } 
        public void ErrorUnique(string message, Exception e)
        {
            var isUnique = 0;
            foreach(string str in errorUniqueListMessage)
            {
                if(str.Contains(message) || str.Contains(e.ToString()))
                {
                    isUnique++;
                }
            }
                if(isUnique > 0)
                {
                    Write(errorPath, errorName, message, e);
                }
                else
                {
                    errorUniqueListMessage.Add(message);
                    errorUniqueListMessage.Add(e.ToString());
                    Write(errorUniquePath, errorUniqueName, message, e);
                }
        }
        public void Warning(string message)
        {
            Write(warningPath, warningName, message);
            warningUniqueListMessage.Add(message);
        }
        public void Warning(string message, Exception e)
        {
            Write(warningPath, warningName, message, e);
            warningUniqueListMessage.Add(message);
        }
        public void WarningUnique(string message)
        {
            var isUnique = 0;
            foreach(string str in warningUniqueListMessage)
            {
                if(str.Contains(message))
                {
                    isUnique++;
                }
            }   
            if(isUnique > 0)
            {
                Write(warningPath, warningName, message);
            }
            else
            {
                warningUniqueListMessage.Add(message);
                Write(warningUniquePath, warningUniqueName, message);
            }
        }
        public void Info(string message)
        {
            Write(infoPath, infoName, message);
        }
        public void Info(string message, Exception e)
        {
            Write(infoPath, infoName, message, e);
        }
        public void Info(string message, params object[] args)
        {
            Write(infoPath, infoName, message, args);
        }
        public void Debug(string message)
        {
            Write(debugPath, debugName, message);
        }
        public void Debug(string message, Exception e)
        {
            Write(debugPath, debugName, message, e);
        }
        public void DebugFormat(string message, params object[] args)
        {
            Write(debugPath, debugName, message, args);
        }
        public void SystemInfo(string message, Dictionary<object, object> properties = null)
        {
            Write(systemInfoPath, systemInfoName, message, properties);
        }
    }
    class Program
    {
        static void Main(string []args)
        {
            var logger = new Logger();
            Logger.isConsoleEnabled = true;
            logger.MainLoop();
            logger.Fatal("Служба доставки упала", new Exception("1354"));
            logger.Error("Сумма получена неверно");
            logger.Warning("Кажется, скоро откажет модуль сборки");
            logger.Info("Пройден путь, но не так", new Exception("Пересчитать путь"));

            Dictionary<object, object> dict = new Dictionary<object, object>();
            dict.Add("System OS", "Windows");
            dict.Add("Ram memory size", "8Gb");
            logger.SystemInfo("systemInfo", dict);
            
            logger.ErrorUnique("Сумма получена неверно", new Exception("ouch..."));
            logger.ErrorUnique("Да такого еще не было, мамой клянусь", new Exception("neverHaveBeen"));
            logger.ErrorUnique("Да такого еще не было, мамой клянусь", new Exception("neverHaveBeen"));

            logger.WarningUnique("Кажется, скоро откажет модуль сборки");
            logger.WarningUnique("А вот это что - то новенькое");
        }
    }   
}