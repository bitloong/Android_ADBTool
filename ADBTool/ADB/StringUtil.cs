using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ADBTool.ADB
{
   public static class StringUtil
    {
       public static string GetSplitString(this string str, string splitString, int index, string defaultString)
       {
           string[] SplitStrs = str.Split(new String[] { splitString }, StringSplitOptions.RemoveEmptyEntries);
           if (SplitStrs == null || SplitStrs.Length-1 < index)
           {
               return defaultString;
           }
           else
           {
               return SplitStrs[index];
           }
       }
       public static string GetStringStartWith(this string[] str, string startString)
       {
           foreach (var item in str)
           {
               if (item.StartsWith(startString))
               {
                   return item;
               }
           }
           return "";
       }
       public static string SubStringAfter(this string source, string startStr)
       {

           MatchCollection matchs = Regex.Matches(source, String.Format(@"(?<={0})[\s\S]*", startStr));
           StringBuilder sb = new StringBuilder();
           foreach (Match item in matchs)
           {
               sb.Append(item.Value);
           }
           string rr = sb.ToString();
           return rr;
       }
    }
}
