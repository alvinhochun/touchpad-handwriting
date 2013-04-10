using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Assembly = System.Reflection.Assembly;
using GuidAttribute = System.Runtime.InteropServices.GuidAttribute;
using Mutex = System.Threading.Mutex;

namespace TouchPadHandwriting
{
    internal static class Program
    {
        internal static bool restartFlag = false;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-HK");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-TW");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("zh-CN");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (Mutex mutex = new Mutex(false, "Global\\" + ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true)[0]).Value))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show(string.Format(Resources.Messages.InstanceAlreadyRunning, Application.ProductName), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    Settings settings = Settings.LoadSettings();
                    if (settings.InkRecognizer == null)
                    {
                        MessageBox.Show(Resources.Messages.NoSuitableRecognizers, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (Properties.Settings.Default.FirstRun)
                        {
                            new FormSettings().ShowDialog();
                        }
                        do
                        {
                            restartFlag = false;
                            Application.Run(new FormMain());
                        } while (restartFlag);
                    }
                }
            }
        }
    }
}
