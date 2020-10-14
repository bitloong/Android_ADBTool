using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADBTool.ADB
{
    /// <summary>
    /// 设备信息
    /// </summary>
    public class Device
    {
        /// <summary>
        /// Android device id
        /// 
        /// </summary>
        public string ID;
        public string status;
        public string product;
        public string model;
        public string device;
        public string transport_id;

        public Device(string deviceStr)
        {
            Set(deviceStr);
        }
        public void Set(string deviceStr)
        {
            string[] devicesStrs = deviceStr.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            ID = devicesStrs[0];
            status = devicesStrs[1];
            product = devicesStrs.GetStringStartWith("product:").GetSplitString( ":", 1, "");
            model = devicesStrs.GetStringStartWith("model:").GetSplitString(":", 1, "");
            device = devicesStrs.GetStringStartWith("device:").GetSplitString(":", 1, "");
            transport_id = devicesStrs.GetStringStartWith("transport_id").GetSplitString(":", 1, "");
        }
    }
}
