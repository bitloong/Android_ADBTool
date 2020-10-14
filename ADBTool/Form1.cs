using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ADBTool.ADB;
using System.IO;

namespace ADBTool
{
    public partial class Form1 : Form
    {
        public static Device SelectedDevice = null;
        List<Device> devices = null;
        int PackagesType = 0;
        public Form1()
        {
            InitializeComponent();
            CmdHelper.callBack = new CmdHelper.CallBack(ShowCmd);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (ADBCommand.CheckADB())
            {
                GetDevicesBtn_Click(null, null);
            }

            if(PackagesTypeComboBox.Items.Count >= 0 )
            {
                PackagesTypeComboBox.SelectedIndex = 0;
            }
        }

        private void ShowCmd(string s)
        {
            ADBCmdTxt.AppendText(s + "\n") ;
            ADBCmdTxt.ScrollToCaret();
        }

        private void ClearCmdTxt_Click(object sender, EventArgs e)
        {
            ADBCmdTxt.Text  = "";
        }

        private void ADBCmdTxt_TextChanged(object sender, EventArgs e)
        {
            ////文本框选中的起始点在最后
            //ADBCmdTxt.SelectionStart = ADBCmdTxt.Text.Length;
            ////ADBCmdTxt.SelectionLength = 0;
            ////将控件内容滚动到当前插入符号位置
            //ADBCmdTxt.ScrollToCaret();

            //让文本框获取焦点
            this.ADBCmdTxt.Focus();
            //设置光标的位置到文本尾
            this.ADBCmdTxt.Select(this.ADBCmdTxt.Text.Length, 0);
            //滚动到控件光标处
            this.ADBCmdTxt.ScrollToCaret();
        }

        /// <summary>
        /// 发送ADB命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExeBtn_Click(object sender, EventArgs e)
        {
            String cmdtxt = cmdTxt.Text.Trim();
            if (cmdtxt != String.Empty)
            {
                CmdHelper.ExecCmd(cmdtxt);
            }
        }
        private void ExeBtnWithS_Click(object sender, EventArgs e)
        {
            String cmdtxt = cmdTxt.Text.Trim();
            ExecCMD(cmdtxt);
        }

        string ExecCMD(string cmdTxt)
        {
            String cmdtxt = cmdTxt.Trim();
            if (devices.Count > 1)
            {
                if (!cmdtxt.Contains("-s "))
                {
                    cmdtxt = cmdtxt.Replace("adb", String.Format("adb -s {0}", SelectedDevice.ID));
                }
            }
            if (cmdtxt != String.Empty)
            {
                return CmdHelper.ExecCmd(cmdtxt);
            }
            else { return ""; }
        }
        /// <summary>
        /// 获取设备按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetDevicesBtn_Click(object sender, EventArgs e)
        {
            DevicesComboBox.Items.Clear();
            DeviceNumTip.Text = String.Format("共{0}个设备", DevicesComboBox.Items.Count);

            devices =  ADBCommand.GetDevices();
            if (devices == null || devices.Count ==0) return;
            foreach (Device item in devices)
            {
                DevicesComboBox.Items.Add(String.Format("{0}_{1}_{2}",item.ID, item.status, item.model));
            }
            DevicesComboBox.SelectedIndex = 0;
            DeviceNumTip.Text = String.Format("共{0}个设备", DevicesComboBox.Items.Count);
        }

        private void DevicesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (devices != null || devices.Count >= DevicesComboBox.SelectedIndex)
            {
                SelectedDevice = devices[DevicesComboBox.SelectedIndex];
            }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProductModelBtn_Click(object sender, EventArgs e)
        {
            ProductModelTxt.Text = "";
            string r = ExecCMD("adb shell getprop ro.product.model");
            string result = CmdHelper.GetCMDResult(r);
            ProductModelTxt.Text = result;

        }

        private void BatteryBtn_Click(object sender, EventArgs e)
        {
            BatteryTxt.Text = "";
            string r = ExecCMD("adb shell dumpsys battery");
            string result = CmdHelper.GetCMDResult(r);
            BatteryTxt.Text = result;

        }

        private void WmSizeBtn_Click(object sender, EventArgs e)
        {
            this.WmSizeTxt.Text = "";
            string r = ExecCMD("adb shell wm size");
            string result = CmdHelper.GetCMDResult(r);
            WmSizeTxt.Text = result;
        }

        private void DensityBtn_Click(object sender, EventArgs e)
        {
            this.DensityTxt.Text = "";
            string r = ExecCMD("adb shell wm density");
            string result = CmdHelper.GetCMDResult(r);
            DensityTxt.Text = result;
        }

        private void DisplaysBtn_Click(object sender, EventArgs e)
        {
            this.DisplaysTxt.Text = "";
            string r = ExecCMD("adb shell dumpsys window displays");
            string result = CmdHelper.GetCMDResult(r);
            DisplaysTxt.Text = result;

        }

        private void Android_IDBtn_Click(object sender, EventArgs e)
        {
            this.Android_IDTxt.Text = "";
            string r = ExecCMD("adb shell settings get secure android_id");
            string result = CmdHelper.GetCMDResult(r);
            Android_IDTxt.Text = result;

        }

        private void IMEIBtn_Click(object sender, EventArgs e)
        {
            this.IMEITxt.Text = "";
            string r = ExecCMD("adb shell dumpsys iphonesubinfo");
            string result = CmdHelper.GetCMDResult(r);
            IMEITxt.Text = result;

        }

        private void AndroidVerBtn_Click(object sender, EventArgs e)
        {
            this.AndroidVerTxt.Text = "";
            string r = ExecCMD("adb shell getprop ro.build.version.release");
            string result = CmdHelper.GetCMDResult(r);
            AndroidVerTxt.Text = result;

        }

        private void IPBtn_Click(object sender, EventArgs e)
        {
            this.IPTxt.Text = "";
            string r = ExecCMD("adb shell netcfg");
            if (CheckCMDResult(r))
            {
                string result = CmdHelper.GetCMDResult(r);
                IPTxt.Text = result;
                return;
            }
            r = ExecCMD("adb shell ifconfig wlan0");
            if (CheckCMDResult(r))
            {
                string result = CmdHelper.GetCMDResult(r);
                IPTxt.Text = result;
                return;
            }
            r = ExecCMD("adb shell ifconfig");
            if (CheckCMDResult(r))
            {
                string result = CmdHelper.GetCMDResult(r);
                IPTxt.Text = result;
                return;
            }
        }

        public bool CheckCMDResult(string result)
        {
            return !result.StartsWith("ADB执行异常");
        }

        private void MacBtn_Click(object sender, EventArgs e)
        {
            this.MacTxt.Text = "";
            string r = ExecCMD(@"adb shell cat /sys/class/net/wlan0/address");
            string result = CmdHelper.GetCMDResult(r);
            MacTxt.Text = result;
        }

        private void CPUBtn_Click(object sender, EventArgs e)
        {
            this.CPUTxt.Text = "";
            string r = ExecCMD(@"adb shell cat /proc/cpuinfo");
            string result = CmdHelper.GetCMDResult(r);
            CPUTxt.Text = result;
        }

        private void MemBtn_Click(object sender, EventArgs e)
        {
            this.MemTxt.Text = "";
            string r = ExecCMD(@"adb shell cat /proc/meminfo");
            string result = CmdHelper.GetCMDResult(r);
            MemTxt.Text = result;
        }

        private void DeviceMoreBtn_Click(object sender, EventArgs e)
        {
            this.DeviceMoreTxt.Text = "";
            string r = ExecCMD(@"adb shell cat /system/build.prop");
            string result = CmdHelper.GetCMDResult(r);
            DeviceMoreTxt.Text = result;
        }

        private void PackagesTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PackagesType = PackagesTypeComboBox.SelectedIndex;
        }

        private void PackagesListBtn_Click(object sender, EventArgs e)
        {
            this.PackagesListComboBox.Text = "";
            this.PackagesListComboBox.Items.Clear();
            PackagesCountTip.Text = String.Format("共{0}个", PackagesListComboBox.Items.Count);
            List<string> packages = null;
            string r = "";
            string result = "";
            switch (PackagesType)
            {
                case 0://第三方应用
                    r = ExecCMD("adb shell pm list packages -3");
                    break;
                case 1://所有应用
                    r = ExecCMD("adb shell pm list packages");
                    break;
                case 2://系统应用
                    r = ExecCMD("adb shell pm list packages -s");
                    break;
                default:
                    break;
            }
            result = CmdHelper.GetCMDResult(r);
            packages = ADBCommand.GetPackagesList(result);

            foreach (var item in packages)
            {
                this.PackagesListComboBox.Items.Add(item);
                PackagesListComboBox.SelectedIndex = 0;
            }
            PackagesCountTip.Text = String.Format("共{0}个", PackagesListComboBox.Items.Count);
        }

        private void PackagesListBtn2_Click(object sender, EventArgs e)
        {
            if (PackagesNameTxt.Text.Trim() == "")
            {
                MessageBox.Show("请输入包名");
                return;
            }
            this.PackagesListComboBox.Text = "";
            this.PackagesListComboBox.Items.Clear();
            PackagesCountTip.Text = String.Format("共{0}个", PackagesListComboBox.Items.Count);
            List<string> packages = null;
            string r = "";
            string result = "";
            r = ExecCMD(String.Format("adb shell pm list packages {0}", PackagesNameTxt.Text.Trim()));
            result = CmdHelper.GetCMDResult(r);
            packages = ADBCommand.GetPackagesList(result);

            foreach (var item in packages)
            {
                this.PackagesListComboBox.Items.Add(item);
                PackagesListComboBox.SelectedIndex = 0;
            }
            PackagesCountTip.Text = String.Format("共{0}个", PackagesListComboBox.Items.Count);
        }

        /// <summary>
        /// 查询应用安装路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PackagePathBtn_Click(object sender, EventArgs e)
        {
            this.PackagePathTxt.Text = "";
            string r = ExecCMD(String.Format(@"adb shell pm path {0}", PackagesListComboBox.Text));
            string result = CmdHelper.GetCMDResult(r);
            PackagePathTxt.Text = result;
        }

        private void PackageInfoBtn_Click(object sender, EventArgs e)
        {
            this.PackageInfoTxt.Text = "";
            string r = ExecCMD(String.Format(@"adb shell dumpsys package {0}", PackagesListComboBox.Text));
            string result = CmdHelper.GetCMDResult(r);
            PackageInfoTxt.Text = result;
        }

        private void SelectPackageBtn_Click(object sender, EventArgs e)
        {
            SelectPackageTxt.Text = "";
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//该值确定是否可以选择多个文件
            dialog.Title = "请选择apk文件";
            dialog.Filter = "apk文件(*.apk)|*.apk";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SelectPackageTxt.Text = dialog.FileName;
            }
        }

        private void InstallBtn_Click(object sender, EventArgs e)
        {
            this.InstallTxt.Text = "";
            string r = ExecCMD(String.Format(@"adb install  -r -t -d {0}", SelectPackageTxt.Text));
            string result = CmdHelper.GetCMDResult(r);
            InstallTxt.Text = result;
        }

        private void ScreencapOutBtn_Click(object sender, EventArgs e)
        {
            String path = @"D:\ScreenCap\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            String pngName = String.Format("{0}{1}.png",path, DateTime.Now.ToString("yyyyMMdd-hhmmss"));
            string r = ExecCMD(String.Format(@"adb exec-out screencap -p > {0}", pngName));
            string result = CmdHelper.GetCMDResult(r);
            this.ScreencapImg.Load(pngName);
            ScreencapOutPathTxt.Text = pngName;
        }

        private void ScreencapBtn_Click(object sender, EventArgs e)
        {
            String path = @"D:\ScreenCap\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            String pngName = String.Format("{0}{1}.png", path, DateTime.Now.ToString("yyyyMMdd-hhmmss"));
            string r = ExecCMD(String.Format(@"adb shell screencap -p /sdcard/sc.png {0}", pngName));
            string result = CmdHelper.GetCMDResult(r);
            this.ScreencapImg.Load(pngName);
            ScreencapOutPathTxt.Text = pngName;
        }

        private void USBSocketSetBtn_Click(object sender, EventArgs e)
        {
            if (this.PCPortTxt.Text.Trim() == "" || this.AppPortTxt.Text.Trim() == "")
            {
                MessageBox.Show("请输入端口号");
                return;
            }
            string r = ExecCMD(String.Format(@"adb forward tcp:{0} tcp:{1}", this.PCPortTxt.Text.Trim(), this.AppPortTxt.Text.Trim()));
            string result = CmdHelper.GetCMDResult(r);

        }

        private void SocketConnectionsBtn_Click(object sender, EventArgs e)
        {
            this.SocketConnectionsTxt.Text = "";
            string r = ExecCMD(@"adb forward --list");
            string result = CmdHelper.GetCMDResult(r);
            SocketConnectionsTxt.Text = result;
        }
    }
}
