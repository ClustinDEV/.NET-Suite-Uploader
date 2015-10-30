using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetsuiteUploader
{
    static class Program
    {
        static Mutex mutex = new Mutex(false, "netsuite-uploader");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!mutex.WaitOne(TimeSpan.FromSeconds(2), false))
            {
                MessageBox.Show("Application already started!", "NOTIFICATION", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }

            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmUploader());
            }
            finally 
            { 
                mutex.ReleaseMutex(); 
            }
        }
    }
}
