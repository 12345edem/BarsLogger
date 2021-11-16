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
        private const string FileExt = ".log";

        private static string _dirPath;
        private string _fatalErrorPath;
        private string _errorPath;
        private string _errorUniquePath;
        private string _warningPath;
        private string _warningUniquePath;
        private string _infoPath;
        private string _debugPath;
        private string _systemInfoPath;

        private const string FatalName = "Fatal";
        private const string ErrorName = "Error";
        private const string ErrorUniqueName = "ErrorUnique";
        private const string WarningName = "Warning";
        private const string WarningUniqueName = "WarningUnique";
        private const string InfoName = "Info";
        private const string DebugName = "Debug";
        private const string SystemInfoName = "SystemInfo";

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
            var slnFilePath = Path.Combine(Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName);
            _dirPath = slnFilePath + "\\LoggerOutput\\" + DateTime.Now.Date.ToString("d");
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
            if(!Directory.Exists(_dirPath))
            {
                Directory.CreateDirectory(_dirPath);
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

            if(File.Exists(_warningUniquePath))
            {
                using(StreamReader sr = new StreamReader(_warningUniquePath))
                    while (!sr.EndOfStream)
                        warningNotUniqueList.Add(sr.ReadLine());
            }

            if(File.Exists(_warningPath))
            {
                using(StreamReader sr = new StreamReader(_warningPath))
                    while(!sr.EndOfStream)
                        warningNotUniqueList.Add(sr.ReadLine());
            }

            if(File.Exists(_errorUniquePath))
            {
                using(StreamReader sr = new StreamReader(_errorUniquePath))
                    while(!sr.EndOfStream)
                        errorNotUniqueList.Add(sr.ReadLine());
            }

            if(File.Exists(_errorPath))
            {
                using(StreamReader sr = new StreamReader(_errorPath))
                    while(!sr.EndOfStream)
                        errorNotUniqueList.Add(sr.ReadLine());
            }
        }

        ///<summary>
        ///Задание путей сохранения по определенным видам логов
        ///</summary>
        private void Paths()
        {
            _fatalErrorPath = _dirPath + @"\" + FatalName + FileExt;
            _errorPath = _dirPath + @"\" + ErrorName + FileExt;
            _errorUniquePath = _dirPath + @"\" + ErrorUniqueName + FileExt;
            _warningPath = _dirPath + @"\" + WarningName + FileExt;
            _warningUniquePath = _dirPath + @"\" + WarningUniqueName + FileExt;
            _infoPath = _dirPath + @"\" + InfoName + FileExt;
            _debugPath = _dirPath + @"\" + DebugName + FileExt;
            _systemInfoPath = _dirPath + @"\" + SystemInfoName + FileExt;
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
            FileWriter.Write(_fatalErrorPath, FatalName, message);
        }
        public void Fatal(string message, Exception e)
        {
            FileWriter.Write(_fatalErrorPath, FatalName, message, e);
        }
        public void Error(string message)
        {
            FileWriter.Write(_errorPath, ErrorName, message);
            errorNotUniqueList.Add(message);
        }
        public void Error(Exception e)
        {
            FileWriter.Write(_errorPath, ErrorName, e);
            errorNotUniqueList.Add(e.ToString());
        }
        public void Error(string message, Exception e)
        {
            FileWriter.Write(_errorPath, ErrorName, message, e);
            errorNotUniqueList.Add(message);
            errorNotUniqueList.Add(e.ToString());
        }
        public void ErrorUnique(string message, Exception e)
        {
            if(!isContains(message, errorNotUniqueList, e.ToString()))
            {
                errorNotUniqueList.Add(message);
                errorNotUniqueList.Add(e.ToString());
                FileWriter.Write(_errorUniquePath, ErrorUniqueName, message, e);
            }
        }
        public void Warning(string message)
        {
            FileWriter.Write(_warningPath, WarningName, message);
            warningNotUniqueList.Add(message);
        }
        public void Warning(string message, Exception e)
        {
            FileWriter.Write(_warningPath, WarningName, message, e);
            warningNotUniqueList.Add(message);
        }
        public void WarningUnique(string message)
        {
            if(!isContains(message, warningNotUniqueList))
            {
                warningNotUniqueList.Add(message);
                FileWriter.Write(_warningUniquePath, WarningUniqueName, message);
            }
        }
        public void Info(string message)
        {
            FileWriter.Write(_infoPath, InfoName, message);
        }
        public void Info(string message, Exception e)
        {
            FileWriter.Write(_infoPath, InfoName, message, e);
        }
        public void Info(string message, params object[] args)
        {
            FileWriter.Write(_infoPath, InfoName, message, args);
        }
        public void Debug(string message)
        {
            FileWriter.Write(_debugPath, DebugName, message);
        }
        public void Debug(string message, Exception e)
        {
            FileWriter.Write(_debugPath, DebugName, message, e);
        }
        public void DebugFormat(string message, params object[] args)
        {
            FileWriter.Write(_debugPath, DebugName, message, args);
        }
        public void SystemInfo(string message, Dictionary<object, object> properties = null)
        {
            FileWriter.Write(_systemInfoPath, SystemInfoName, message, properties);
        }
    }
}