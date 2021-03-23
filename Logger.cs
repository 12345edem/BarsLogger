using System;
using System.IO;
using System.Collections.Generic;

namespace BarsLogger
{
    public class Logger : ILog 
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

        //Наименования логов для вывода в Console и задания имени выходного файла
        private const string fatalName = "Fatal";
        private const string errorName = "Error";
        private const string errorUniqueName = "ErrorUnique";
        private const string warningName = "Warning";
        private const string warningUniqueName = "Warning Unique";
        private const string infoName = "Info";
        private const string debugName = "Debug";
        private const string systemInfoName = "SystemInfo";

        //Списки для проверки уникальных ошибок и ворнингов
        private static List<string> errorUniqueListMessage = new List<string>();
        public static List<string> warningUniqueListMessage = new List<string>();
        
        public Logger()
        {
            dirPath = @"Logger\" + DateTime.Now.Date.ToString("d");
            MainLoop();
            Paths();
            Init();
        }
        //Запускаем это в цикле для проверки смены дня, работает и при запуске программы
        public void MainLoop()
        {
            if(!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                Init();
            }
        }

        //Инициализация: очистка списков, заполнение списков неуникальными значениями
        private void Init()
        {
            errorUniqueListMessage.Clear();
            warningUniqueListMessage.Clear();

            //Учет уникальных ворнингов
            try
            {
                
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
            fatalErrorPath = dirPath + @"\" + fatalName + fileExt;
            errorPath = dirPath + @"\" + errorName + fileExt;
            errorUniquePath = dirPath + @"\" + errorUniqueName + fileExt;
            warningPath = dirPath + @"\" + warningName + fileExt;
            warningUniquePath = dirPath + @"\" + warningUniqueName + fileExt;
            infoPath = dirPath + @"\" + infoName + fileExt;
            debugPath = dirPath + @"\" + debugName + fileExt;
            systemInfoPath = dirPath + @"\" + systemInfoName + fileExt;
        }

        //Реализация методов интерфейса ILog
        public void Fatal(string message)
        {
            FileWriter.Write(fatalErrorPath, fatalName, message);
        }
        public void Fatal(string message, Exception e)
        {
            FileWriter.Write(fatalErrorPath, fatalName, message, e);
        }
        public void Error(string message)
        {
            FileWriter.Write(errorPath, errorName, message);
            errorUniqueListMessage.Add(message);
        }
        public void Error(Exception e)
        {
            FileWriter.Write(errorPath, errorName, e);
            errorUniqueListMessage.Add(e.ToString());
        }
        public void Error(string message, Exception e)
        {
            FileWriter.Write(errorPath, errorName, message, e);
            errorUniqueListMessage.Add(message);
            errorUniqueListMessage.Add(e.ToString());
        } 
        public void ErrorUnique(string message, Exception e)
        {
            if(isUniqueCheck.isUnique(message, e, errorUniqueListMessage))
            {
                errorUniqueListMessage.Add(message);
                errorUniqueListMessage.Add(e.ToString());
                FileWriter.Write(errorUniquePath, errorUniqueName, message, e);
            }
            else
            {
                FileWriter.Write(errorPath, errorName, message, e);
            }
        }
        public void Warning(string message)
        {
            FileWriter.Write(warningPath, warningName, message);
            warningUniqueListMessage.Add(message);
        }
        public void Warning(string message, Exception e)
        {
            FileWriter.Write(warningPath, warningName, message, e);
            warningUniqueListMessage.Add(message);
        }
        public void WarningUnique(string message)
        {
            if(isUniqueCheck.isUnique(message, warningUniqueListMessage))
            {
                warningUniqueListMessage.Add(message);
                FileWriter.Write(warningUniquePath, warningUniqueName, message);
            }
            else
            {
                FileWriter.Write(warningPath, warningName, message);
            }
        }
        public void Info(string message)
        {
            FileWriter.Write(infoPath, infoName, message);
        }
        public void Info(string message, Exception e)
        {
            FileWriter.Write(infoPath, infoName, message, e);
        }
        public void Info(string message, params object[] args)
        {
            FileWriter.Write(infoPath, infoName, message, args);
        }
        public void Debug(string message)
        {
            FileWriter.Write(debugPath, debugName, message);
        }
        public void Debug(string message, Exception e)
        {
            FileWriter.Write(debugPath, debugName, message, e);
        }
        public void DebugFormat(string message, params object[] args)
        {
            FileWriter.Write(debugPath, debugName, message, args);
        }
        public void SystemInfo(string message, Dictionary<object, object> properties = null)
        {
            FileWriter.Write(systemInfoPath, systemInfoName, message, properties);
        }
    }
}