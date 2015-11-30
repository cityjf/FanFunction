using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace FanFunction
{
    /// <summary>
    /// 关于操作系统相关的函数
    /// </summary>
    public class clsOS
    {
        //执行相关操作，调用API
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern long ExitWindowsEx(long uFlags, long dwReserved);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);
        /// <summary>
        /// 关机
        /// </summary>
        public static void Shutdown()
        {
            ExitWindowsEx(1, 0);
        }
        /// <summary>
        /// 重启
        /// </summary>
        public static void Restart()
        {
            ExitWindowsEx(2, 0);
        }
        /// <summary>
        /// 注销
        /// </summary>
        public static void Off()
        {
            ExitWindowsEx(0, 0);
        }
        /// <summary>
        /// 关闭显示器
        /// </summary>
        public static void OffScreen()
        {
            SendMessage(-1, 0x0112, 0xF170, 2);
        }
        /// <summary>
        /// 打开显示器
        /// </summary>
        public static void OnScreen()
        {
            SendMessage(-1, 0x0112, 0xF170, -1);
        }
        /// <summary>
        /// 结束指定名称的进程,多个以","隔开
        /// </summary>
        /// <param name="strProcessName"></param>
        public void KillProcess(string strProcessName)
        {
            string[] strAllProcess = strProcessName.Split(',');
            foreach (string strProcess in strAllProcess)
            {
                if (!string.IsNullOrEmpty(strProcess))
                {
                    foreach (Process thisProcess in Process.GetProcesses())
                    {
                        if (thisProcess.ProcessName.Equals(strProcessName))
                        {
                            thisProcess.Kill();
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 结束指定pid的进程
        /// </summary>
        /// <param name="ProcessID"></param>
        public void KillProcess(int ProcessID)
        {
            foreach (Process thisProcess in Process.GetProcesses())
            {
                if (thisProcess.Id.Equals(ProcessID))
                {
                    thisProcess.Kill();
                    break;
                }
            }
        }
        /// <summary>
        /// 结束指定pid数组的进程
        /// </summary>
        /// <param name="ProcessIDs"></param>
        public void KillProcessByLstID(int[] ProcessIDs)
        {
            foreach (int ProcessID in ProcessIDs)
            {
                foreach (Process thisProcess in Process.GetProcesses())
                {
                    if (thisProcess.Id.Equals(ProcessID))
                    {
                        thisProcess.Kill();
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 打开网页
        /// </summary>
        /// <param name="strWebsite">网址</param>
        public void OpentheWeb(string strWebsite)
        {
            Process.Start(strWebsite);
        }
        /// <summary>
        /// 用指定浏览器打开网页
        /// </summary>
        /// <param name="strBrowser">哪种浏览器(IE是:iexplore)</param>
        /// <param name="strWebsite">网址</param>
        public void OpentheWeb(string strBrowser, string strWebsite)
        {
            Process.Start(strBrowser, strWebsite);
        }
        /// <summary>
        /// 打开指定应用程序
        /// </summary>
        /// <param name="strExePath">应用程序完整路径</param>
        public void OpentheExe(string strExePath)
        {
            Process.Start(strExePath);
        }
    }
}
