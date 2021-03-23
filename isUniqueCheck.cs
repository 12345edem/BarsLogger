using System;
using System.Collections.Generic;

namespace BarsLogger
{
    //Немного магии по определению наличия в списке переданной строки
    public class isUniqueCheck
    {
        public static bool isUnique(string message, Exception e, List<string> errorUniqueListMessage)
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
                    return false;
                }
                else
                {
                    return true;
                }
        }
        public static bool isUnique(string message, List<string> warningUniqueListMessage)
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
                return false;
            }
            else
            {
               return true;
            }
        } 
    }
}