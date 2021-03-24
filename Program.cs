using System;
using System.IO;
using System.Collections.Generic;

namespace BarsLogger
{
    //Проверочный прогон написанного Логгера
    class Program
    {
        static void Main(string []args)
        {
            var logger = new Logger();
            FileWriter.isConsoleEnabled = false;

            //При работе в цикле, вызвать в нем <MainLoop()>
            //logger.MainLoop();

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
            logger.WarningUnique("Да точно новое");
        }
    }   
}
/*
-Внести isUnque как отдельный метод
-Переименовать неуникальные списки
*/