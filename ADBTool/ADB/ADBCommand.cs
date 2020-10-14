using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ADBTool.ADB
{
    public class ADBCommand
    {
        static public Dictionary<String, String> dicADBDebuggingCommands = new Dictionary<String, String>{
            {"devices","adb devices"},
            {"forward","adb forward"},
            {"kill-server","adb kill-server"}
        };
        static public Dictionary<String, String> dicADBWirelessCommands = new Dictionary<String, String>{
            {"connect","adb connect"},
            {"usb","adb usb"}
        };
        static public Dictionary<String, String> dicADBPackageManagerCommands = new Dictionary<String, String>{
            {"install","adb install"},
            {"uninstall","adb uninstall"},
            {"list","adb shell pm list packages"},
            {"path","adb shell pm path"},
            {"clear","adb shell pm clear"}
        };
        static public Dictionary<String, String> dicADBFileManagerCommands = new Dictionary<String, String>{
            {"pull","adb  pull"},
            {"push","adb push"},
            {"ls","adb shell ls"},
            {"cd","adb shell cd"},
            {"rm","adb shell rm"},
            {"mkdir","adb shell mkdir"},
            {"touch","adb shell touch"},
            {"pwd","adb shell pwd"},
            {"cp","adb shell cp"},
            {"mv","adb shell mv"}
        };
        static public Dictionary<String, String> dicADBLogcatCommands = new Dictionary<String, String>{
            {"logcat","adb logcat"},
            {"dumpsys","adb shell dumpsys"},
            {"dumpstate","adb shell dumpstate"}
        };
        static public Dictionary<String, String> dicADBScreenshotCommands = new Dictionary<String, String>{
            {"screencap","adb shell screencap"}
        };
        static public Dictionary<String, String> dicADBSystemCommands = new Dictionary<String, String>{
            {"root","adb root"},
            {"sideload","adb sideload"},
            {"ps","adb shell ps"},
            {"top","adb shell top"},
            {"getprop","adb shell getprop"},
            {"setprop","adb shell setprop"}
        };
        public ADBCommand()
        { }

        /// <summary>
        /// 检查是否正常安装ADB
        /// </summary>
        /// <returns></returns>
        static public bool CheckADB()
        {
            //string r = CmdHelper.ExecCmd("ipconfig");
            string r = CmdHelper.ExecCmd("adb");
            if (r.Contains("不是内部或外部命令") || r.Contains("is not recognized as an internal or external command"))
                return false;
            return true;
        }

        static public List<Device> GetDevices()
        {
            List<Device> devices = new List<Device>();
            string cmd = String.Format("{0} -l", dicADBDebuggingCommands["devices"]);
            string r = CmdHelper.ExecCmd(cmd);
            string result = CmdHelper.GetCMDResult(r);

            string devicesStr = result.SubStringAfter("List of devices attached");

            //c66e333b               device product:dipper model:MI_8 device:dipper transport_id:51
            string[] devicesStrs = devicesStr.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var deviceStr in devicesStrs)
            {
                devices.Add(new Device(deviceStr));
            }
            return devices;
        }

        /// <summary>
        /// 解析GetPackagesList返回结果
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        static public List<string> GetPackagesList(string result)
        {
            List<string> packageList = new List<string>();

            string[] packages = result.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in packages)
            {
                packageList.Add(item.GetSplitString(":", 1, ""));
            }
            return packageList;
        }

    }
}
