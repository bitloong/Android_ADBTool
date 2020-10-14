using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ADBTool.ADB
{
    public class CmdHelper
    {
        public delegate void CallBack(string str);//定义一个委托
        public static CallBack callBack = null;

        public CmdHelper(CallBack _callBack)
        {
            callBack = _callBack;
        }
        public static void callCallBack(string s)
        {
            if (callBack == null) return;
            callBack(s);
        }
        /// <summary>
        /// 执行cmd命令
        /// </summary>
        /// <param name="cmds"></param>
        /// <returns></returns>
        public static string ExecCmd(List<string> cmds)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
            p.StartInfo.RedirectStandardError = true;   //接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardInput = true;   //由调用程序获取输出信息
            p.StartInfo.RedirectStandardOutput = true;  //重定向标准错误输出
            p.StartInfo.CreateNoWindow = true;          //不显示程序窗口
            //pro.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            p.Start();
            foreach (var item in cmds)
            {
                p.StandardInput.WriteLine(item);
                callCallBack(item + "\n");
            }
            p.StandardInput.WriteLine("exit");
            callCallBack("exit" + "\n");
            p.StandardInput.AutoFlush = true;
            string errOutput = p.StandardError.ReadToEnd();
            string output = p.StandardOutput.ReadToEnd();
            //获取cmd窗口的输出信息

            //超时1分钟
            p.WaitForExit(1000 * 60);//等待程序执行完退出进程
            p.Close();
            //安装apk的时候 Kb/s 会被识别为错误信息
            if (!string.IsNullOrEmpty(errOutput) && !errOutput.Contains("KB/s"))
            {
                output = "ADB执行异常:" + errOutput;
            }
            callCallBack(output);
            return output;

        }
        /// <summary>
        /// 执行cmd命令
        /// </summary>
        /// <param name="cmds"></param>
        /// <returns></returns>
        //public static string ExecCmd(string cmd)
        //{
        //    string output = "";
        //    try
        //    {
        //        System.Diagnostics.Process p = new System.Diagnostics.Process();
        //        p.StartInfo.FileName = "adb.exe";
        //        p.StartInfo.Arguments = "";    //设定程式执行參數  
        //        p.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
        //        p.StartInfo.RedirectStandardError = true;   //接受来自调用程序的输入信息
        //        p.StartInfo.RedirectStandardInput = true;   //由调用程序获取输出信息
        //        p.StartInfo.RedirectStandardOutput = true;  //重定向标准错误输出
        //        p.StartInfo.CreateNoWindow = true;          //不显示程序窗口
        //        //pro.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        //        p.Start();

        //        callCallBack("______________________________" + Environment.NewLine);
        //        callCallBack(">>" + Environment.NewLine);
        //        //p.StandardInput.WriteLine(cmd+"&exit");
        //        callCallBack(cmd + Environment.NewLine);

        //        p.StandardInput.WriteLine("&exit");
        //        callCallBack("exit" + Environment.NewLine);
        //        p.StandardInput.AutoFlush = true;
        //        //获取cmd窗口的输出信息
        //        output = p.StandardOutput.ReadToEnd();
        //        string errOutput = p.StandardError.ReadToEnd();

        //        //超时1分钟
        //        p.WaitForExit(1000 * 1);//等待程序执行完退出进程
        //        p.Close();
        //        //安装apk的时候 Kb/s 会被识别为错误信息
        //        callCallBack("<<" + Environment.NewLine);
        //        if (!string.IsNullOrEmpty(errOutput) && !errOutput.Contains("KB/s"))
        //        {
        //            output = "ADB执行异常:" + errOutput;
        //        }
        //        callCallBack(output +Environment.NewLine);
        //        callCallBack("______________________________");
        //    }
        //    catch (System.Exception ex)
        //    {
        //        output = ex.Message;
        //        callCallBack(output + Environment.NewLine);
        //    }
        //    return output;

        //}
        public static string ExecCmd(string cmd)
        {
            StringBuilder output = new StringBuilder();
            try
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.FileName = "cmd.exe";
                //p.StartInfo.Arguments = "";    //设定程式执行參數  
                p.StartInfo.UseShellExecute = false;        //是否使用操作系统shell启动
                p.StartInfo.RedirectStandardError = true;   //接受来自调用程序的输入信息
                p.StartInfo.RedirectStandardInput = true;   //由调用程序获取输出信息
                p.StartInfo.RedirectStandardOutput = true;  //重定向标准错误输出
                p.StartInfo.CreateNoWindow = true;          //不显示程序窗口
                //pro.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                p.Start();

                callCallBack("______________________________" + Environment.NewLine);
                callCallBack(">>" + Environment.NewLine);
                p.StandardInput.WriteLine(cmd + "&exit");
                callCallBack(cmd + Environment.NewLine);

                //p.StandardInput.WriteLine("&exit");
                callCallBack("exit" + Environment.NewLine);
                p.StandardInput.AutoFlush = true;

                p.StandardInput.Close();   //运行完毕关闭控制台输入
                //超时1分钟
                bool b=p.WaitForExit(1000 * 1);//等待程序执行完退出进程
                //获取cmd窗口的输出信息
                output.Append(p.StandardOutput.ReadToEnd()) ;
                p.StandardOutput.Close();

                //int peek = p.StandardOutput.Peek();
                //while (peek > -1)
                //{
                //    char[] buffer = new char[peek];
                //    p.StandardOutput.ReadBlock(buffer, 0, peek);
                //    //output.Append( + Environment.NewLine);
                //    peek = p.StandardOutput.Peek();
                //}
                string errOutput = p.StandardError.ReadToEnd();
                p.Close();
                //安装apk的时候 Kb/s 会被识别为错误信息
                callCallBack("<<" + Environment.NewLine);
                if (!string.IsNullOrEmpty(errOutput) && !errOutput.Contains("KB/s"))
                {
                    output.Append("ADB执行异常:" + errOutput);
                }
                string outString = output.ToString().SubStringAfter("保留所有权利。");
                callCallBack(outString + Environment.NewLine);
                callCallBack("______________________________");
            }
            catch (System.Exception ex)
            {
                output.Append(ex.Message) ;
                callCallBack(output + Environment.NewLine);
            }
            return output.ToString();

        }

        public static string GetCMDResult(string r)
        {
            MatchCollection matchs = Regex.Matches(r, @"(?<=exit)[\s\S]*");
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
