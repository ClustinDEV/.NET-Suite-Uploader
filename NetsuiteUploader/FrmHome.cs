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
using System.Diagnostics;

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

        private void frmUploader_Load(object sender, EventArgs e)
        {
            loadAccounts();
            executeLogin(null);
        }

        private void netSuiteService_addListCompleted(object sender, addListCompletedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                appendLog("Upload done!", Color.Green);
                btnUpload.Enabled = true;
                btnUpload.Cursor = Cursor.Current = Cursors.Default;
            }));
        }

        private void frmUploader_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.U)
            {
                upload();
            }
        }

        private void frmUploader_Closing(object sender, FormClosingEventArgs e)
        {
            Login login = new Login();
            login.logout(netSuiteService, sessionResponse);
        }

        private void tmrTimeout_Tick(object sender, EventArgs e)
        {
            GetServerTimeResult gstr = netSuiteService.getServerTime();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            upload();
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            System.Threading.Thread.Sleep(2000);

            FileUploader fileUploader = new FileUploader();
            fileUploader.UploadFiles(netSuiteService, currentTask);
        }

        private void ddbAccount_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Login login = new Login();
            login.logout(netSuiteService, sessionResponse);

            executeLogin(e.ClickedItem.ToString());
        }

        private void chkWatchChanges_CheckedChanged(object sender, EventArgs e)
        {
            upload();
        }

        private void mniTasksOpenFolder_Click(object sender, EventArgs e)
        {
            string taskFolder = ConfigurationManager.AppSettings["taskfolder"].ToString().TrimEnd('\\');
            Process.Start("explorer.exe", taskFolder);
        }

        private void mniTasksOpenFile_Click(object sender, EventArgs e)
        {
            if (null != lstTasks.SelectedItem)
            {
                string taskFile = ConfigurationManager.AppSettings["taskfolder"].ToString().TrimEnd('\\') + @"\" + lstTasks.SelectedItem;
                Process.Start(taskFile);
            }
            else
            {
                appendLog("Please select a task", Color.Yellow);
            }
        }

        private void loadTasks()
        {
            lstTasks.Items.Clear();
            string taskFolder = ConfigurationManager.AppSettings["taskfolder"].ToString().TrimEnd('\\');
            string[] filePaths = Directory.GetFiles(taskFolder);
            for (int i = 0; i < filePaths.Length; i++)
            {
                string fileName = System.IO.Path.GetFileName(filePaths[i]);
                lstTasks.Items.Add(fileName);
            }
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
                    if (chkWatchChanges.Checked)
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
                catch (Exception ex)
                {
                    appendLog(ex.Message, Color.Red);
                }
            }
            else
            {
                appendLog("Please select a task", Color.Yellow);
            }
        }

        private void appendLog(string log)
        {
            appendLog(log, Color.Transparent);
        }

        private void appendLog(string log, Color backColor)
        {
            DateTime now = System.DateTime.Now;
            Invoke(new Action(() =>
            {
                string message = now.ToShortDateString() + " " + now.ToShortTimeString() + ":" + now.Second + " " + log + "\r\n";
                txtLog.Text = message + txtLog.Text;

                pnlStatus.BackColor = backColor;
 
            }));

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(8000);
                Invoke(new Action(() => ClearStatus()));
            });
        }

        private object ClearStatus()
        {
            pnlStatus.BackColor = Color.Transparent;
            return null;
        }

        private void loadAccounts()
        {
            string accounts = System.Configuration.ConfigurationManager.AppSettings["account"].ToString();
            string[] arrAccount = accounts.Split(',');
            for (int i = 0; i < arrAccount.Length; i++)
                ddbAccount.DropDownItems.Add(arrAccount[i]);
        }

        private void executeLogin(string account)
        {
            Login login = new Login();
            sessionResponse = (account != null) ? login.login(netSuiteService, account) : login.login(netSuiteService);
            if (sessionResponse != null && sessionResponse.status.isSuccess)
            {
                lblToolStripStatus.Text = "Account: " + login.Account + " - Email: " + login.Email;
                appendLog("Login done [" + lblToolStripStatus.Text + "]");

                if(account == null)
                {
                    netSuiteService.addListCompleted += netSuiteService_addListCompleted;
                    loadTasks();
                }
            }
            else
            {
                tmrTimeout.Stop();

                lblToolStripStatus.Text = "Login failed!";
                appendLog(lblToolStripStatus.Text, Color.Red);
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
                foreach (FileSystemWatcher watcher in listFileSystemWatcher)
                {
                    watcher.EnableRaisingEvents = false;
                    watcher.Dispose();
                }
            }

            listFileSystemWatcher = null;
        }

        
        
    }
}
