using System;
using System.IO;
using System.Collections.Generic;

namespace BarsLogger
{
    public class Logger : ILog 
    {
        ///<summary>
        ///Расширение файла
        ///</summary>
        private const string fileExt = ".log";

        private static string dirPath;
        private string fatalErrorPath;
        private string errorPath;
        private string errorUniquePath;
        private string warningPath;
        private string warningUniquePath;
        private string infoPath;
        private string debugPath;
        private string systemInfoPath;

        private const string fatalName = "Fatal";
        private const string errorName = "Error";
        private const string errorUniqueName = "ErrorUnique";
        private const string warningName = "Warning";
        private const string warningUniqueName = "Warning Unique";
        private const string infoName = "Info";
        private const string debugName = "Debug";
        private const string systemInfoName = "SystemInfo";

        ///<summary>
        ///Список для хранения неуникальных ошибок в процессе работы и при инициализации
        ///</summary>
        private static List<string> errorNotUniqueList = new List<string>();
        ///<summary>
        ///Список для хранения неуникальных ворнингов в процессе работы и при инициализации
        ///</summary>
        private static List<string> warningNotUniqueList = new List<string>();
        
        public Logger()
        {
            dirPath = @"Logger\" + DateTime.Now.Date.ToString("d");
            MainLoop();
            Paths();
            Init();
        }
        
        ///<summary>
        ///Цикл: обновление информации по необходимости создания новой папки,
        ///создание новой папки каждый день с последующей инициализацией логгера
        ///</summary>
        
        public void MainLoop()
        {
            if(!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                errorNotUniqueList.Clear();
                warningNotUniqueList.Clear();
            }
        }

        ///<summary>
        ///Инициализация: очистка списков, заполнение списков неуникальных значений
        ///</summary>
        private void Init()
        {
            errorNotUniqueList.Clear();
            warningNotUniqueList.Clear();

            if(File.Exists(warningUniquePath))
            {
                using(StreamReader sr = new StreamReader(warningUniquePath))
                    while (!sr.EndOfStream)
                        warningNotUniqueList.Add(sr.ReadLine());
            }
            
            if(File.Exists(warningPath))
            {
                using(StreamReader sr = new StreamReader(warningPath))
                    while(!sr.EndOfStream)
                        warningNotUniqueList.Add(sr.ReadLine());
            }
            
            if(File.Exists(errorUniquePath))
            {
                using(StreamReader sr = new StreamReader(errorUniquePath))
                    while(!sr.EndOfStream)
                        errorNotUniqueList.Add(sr.ReadLine());
            }
            
            if(File.Exists(errorPath))
            {
                using(StreamReader sr = new StreamReader(errorPath))
                    while(!sr.EndOfStream)
                        errorNotUniqueList.Add(sr.ReadLine());
            }
        }

        ///<summary>
        ///Задание путей сохранения по определенным видам логов
        ///</summary>
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

        ///<summary>
        ///Проверка наличия в списке переданной строки
        ///</summary>
        private static bool isContains(string message, List<string> notUniqueElemList,  string e = "__*unique*__")
        {
            foreach(string str in notUniqueElemList)
            {
                if(str.Contains(message) || str.Contains(e))
                    return true;
            }   
            return false;
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
            errorNotUniqueList.Add(message);
        }
        public void Error(Exception e)
        {
            FileWriter.Write(errorPath, errorName, e);
            errorNotUniqueList.Add(e.ToString());
        }
        public void Error(string message, Exception e)
        {
            FileWriter.Write(errorPath, errorName, message, e);
            errorNotUniqueList.Add(message);
            errorNotUniqueList.Add(e.ToString());
        } 
        public void ErrorUnique(string message, Exception e)
        {
            if(!isContains(message, errorNotUniqueList, e.ToString()))
            {
                errorNotUniqueList.Add(message);
                errorNotUniqueList.Add(e.ToString());
                FileWriter.Write(errorUniquePath, errorUniqueName, message, e);
            }  
        }
        public void Warning(string message)
        {
            FileWriter.Write(warningPath, warningName, message);
            warningNotUniqueList.Add(message);
        }
        public void Warning(string message, Exception e)
        {
            FileWriter.Write(warningPath, warningName, message, e);
            warningNotUniqueList.Add(message);
        }
        public void WarningUnique(string message)
        {
            if(!isContains(message, warningNotUniqueList))
            {
                warningNotUniqueList.Add(message);
                FileWriter.Write(warningUniquePath, warningUniqueName, message);
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