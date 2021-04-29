using Client_Server_Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Host
{
    class Program
    {
        [DllImport("User32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow([In] IntPtr hWnd, [In] int nCmdShow);
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(Notification_Service)))
            {            
                host.Open();
                Console.WriteLine("Хост запущен");
                IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;

                ShowWindow(handle, 6);
                Console.ReadLine();
            }
        }
    }
}
