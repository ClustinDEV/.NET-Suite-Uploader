using NetsuiteUploader.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;
using NetsuiteUploader.com.netsuite.na1.webservices;
using System.Net;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;

namespace NetsuiteUploader
{
    public partial class frmUploader : Form
    {
        private NetSuiteService netSuiteService = new NetSuiteService();
        private SessionResponse sessionResponse;
        private string currentTask;
        private List<FileSystemWatcher> listFileSystemWatcher = null; 

        public frmUploader()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Login login = new Login();
            sessionResponse = login.login(netSuiteService);
            if(sessionResponse != null && sessionResponse.status.isSuccess)
            {
                lblToolStripStatus.Text = "Account: " + login.Account + " - Email: " + login.Email;
                appendLog("Login done [" + lblToolStripStatus.Text + "]");
                netSuiteService.addListCompleted += netSuiteService_addListCompleted;
                loadTasks();
            }
            else
            {
                tmrTimeout.Stop();

                lblToolStripStatus.Text = "Login failed!";
                appendLog(lblToolStripStatus.Text);
            }

        }

        private void netSuiteService_addListCompleted(object sender, addListCompletedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                appendLog("Upload done!");
                btnUpload.Enabled = true;
                btnUpload.Cursor = Cursor.Current = Cursors.Default;
            }));
        }

        private void Form1_Closing(object sender, FormClosingEventArgs e)
        {
            if (sessionResponse != null && sessionResponse.status.isSuccess)
                netSuiteService.logout();
        }

        private void tmrTimeout_Tick(object sender, EventArgs e)
        {
            GetServerTimeResult gstr = netSuiteService.getServerTime();
        }


        private void loadTasks() 
        {
            string[] filePaths = Directory.GetFiles(ConfigurationSettings.AppSettings["taskfolder"].ToString());
            for (int i = 0; i < filePaths.Length; i++)
            {
                string fileName = System.IO.Path.GetFileName(filePaths[i]);
                lstTasks.Items.Add(fileName);
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            upload();
        }

        private void upload()
        {
            if (null != lstTasks.SelectedItem)
            {
                try
                { 
                    btnUpload.Enabled = false;
                    btnUpload.Cursor = Cursor.Current = Cursors.WaitCursor;

                    currentTask = lstTasks.SelectedItem.ToString();

                    FileUploader fileUploader = new FileUploader();
                    TaskFile[] taskFiles = fileUploader.UploadFiles(netSuiteService, currentTask);

                    List<string> filePaths = taskFiles.Select(f => System.IO.Path.GetDirectoryName(f.Path)).Distinct().ToList();
                    if(chkWatchChanges.Checked)
                    {
                        listFileSystemWatcher = new List<FileSystemWatcher>();
                        foreach (string filePath in filePaths)
                            watch(filePath);
                    }
                    else
                    {
                        unWatch();
                    }
                }
                catch(Exception ex)
                {
                    appendLog(ex.Message);
                }
            }
            else
            {
                appendLog("Please select a task");
            }
        }

        private void watch(string path)
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.*";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;

            listFileSystemWatcher.Add(watcher);
        }

        private void unWatch()
        {
            if (listFileSystemWatcher != null)
            { 
                foreach(FileSystemWatcher watcher in listFileSystemWatcher)
                { 
                    watcher.EnableRaisingEvents = false;
                    watcher.Dispose();
                }
            }

            listFileSystemWatcher = null;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            System.Threading.Thread.Sleep(2000);

            FileUploader fileUploader = new FileUploader();
            fileUploader.UploadFiles(netSuiteService, currentTask);
        }

        private void appendLog(string log)
        {
            DateTime now = System.DateTime.Now;
            Invoke(new Action(() =>
            {
                txtLog.Text = now.ToShortDateString() + " " + now.ToShortTimeString() + ":" + now.Second + " " + log + "\r\n" + txtLog.Text;
            }));
        }
    }
}
