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
using System.Reflection;

namespace NetsuiteUploader
{
    public partial class frmUploader : Form
    {
        private Assembly NetsuiteUploaderAssembly = Assembly.GetExecutingAssembly();

        private NetSuiteService netSuiteService = new NetSuiteService();
        
        private SessionResponse sessionResponse;
        
        private string currentTask;
        
        private List<FileSystemWatcher> listFileSystemWatcher = null;
        
        private const string NOTIFICATION_SUCCESS = "1";
        
        private const string NOTIFICATION_ERROR = "2";

        public frmUploader()
        {
            InitializeComponent();
        }

        #region CONTROLS EVENTS

        private void frmUploader_Load(object sender, EventArgs e)
        {
            loadAccounts();
            executeLogin(null);

            loadResources();
        }

        private void netSuiteService_addListCompleted(object sender, addListCompletedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                bool success = true;

                if(!e.Result.status.isSuccess)
                {
                    success = false;
                    appendLog(string.Format("Upload error: {0}", e.Error), NOTIFICATION_ERROR);
                }

                foreach(WriteResponse wr in e.Result.writeResponse)
                {
                    if(!wr.status.isSuccess)
                    {
                        success = false;
                        appendLog(string.Format("Upload error: {0}", wr.status.statusDetail[0].message), NOTIFICATION_ERROR);
                    }
                }

                if(success)
                    appendLog("Upload done!", NOTIFICATION_SUCCESS);


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
            try
            {
                System.Threading.Thread.Sleep(2000);

                FileUploader.UploadFiles(netSuiteService, currentTask);
            }
            catch (Exception ex)
            {
                appendLog(ex.Message, NOTIFICATION_ERROR);
            }
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
            if (null != cboTasks.SelectedItem)
            {
                string taskFile = ConfigurationManager.AppSettings["taskfolder"].ToString().TrimEnd('\\') + @"\" + cboTasks.SelectedItem.ToString();
                Process.Start(taskFile);
            }
            else
            {
                appendLog("Please select a task", NOTIFICATION_ERROR);
            }
        }

        #endregion

        #region PRIVATE FUNCTIONS

        /// <summary>
        /// load the list of tasks present in the tasks folder
        /// </summary>
        private void loadTasks()
        {
            cboTasks.Items.Clear();
            string taskFolder = ConfigurationManager.AppSettings["taskfolder"].ToString().TrimEnd('\\');
            string[] filePaths = Directory.GetFiles(taskFolder);
            for (int i = 0; i < filePaths.Length; i++)
            {
                string fileName = System.IO.Path.GetFileName(filePaths[i]);
                cboTasks.Items.Add(fileName);
            }
        }

        /// <summary>
        /// call the upload service and config the file watcher
        /// </summary>
        private void upload()
        {
            if (null != cboTasks.SelectedItem)
            {
                try
                {
                    btnUpload.Enabled = false;
                    btnUpload.Cursor = Cursor.Current = Cursors.WaitCursor;

                    currentTask = cboTasks.SelectedItem.ToString();

                    TaskFile[] taskFiles = FileUploader.UploadFiles(netSuiteService, currentTask);

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
                    appendLog(ex.Message, NOTIFICATION_ERROR);
                }
            }
            else
            {
                appendLog("Please select a task", NOTIFICATION_ERROR);
            }
        }

        /// <summary>
        /// add new log line
        /// </summary>
        /// <param name="log">log message</param>
        private void appendLog(string log)
        {
            appendLog(log, string.Empty);
        }

        /// <summary>
        /// add new log line with a type for show an execution status image
        /// </summary>
        /// <param name="log">log message</param>
        /// <param name="notification_type">type of message</param>
        private void appendLog(string log, string notification_type)
        {
            picDone.Visible = picError.Visible = false;

            DateTime now = System.DateTime.Now;
            Invoke(new Action(() =>
            {
                string message = now.ToShortDateString() + " " + now.ToShortTimeString() + ":" + now.Second + " " + log + "\r\n";
                txtLog.Text = message + txtLog.Text;

                if (NOTIFICATION_SUCCESS == notification_type)
                    picDone.Visible = true;
                else if (NOTIFICATION_ERROR == notification_type)
                    picError.Visible = true;
            }));

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(8000);
                Invoke(new Action(() => ClearStatus()));
            });
        }

        /// <summary>
        /// clear the execution status image
        /// </summary>
        /// <returns></returns>
        private object ClearStatus()
        {
            picDone.Visible = picError.Visible = false;
            return null;
        }

        /// <summary>
        /// load the selectable accounts
        /// </summary>
        private void loadAccounts()
        {
            Stream myStream = NetsuiteUploaderAssembly.GetManifestResourceStream("NetsuiteUploader.Resources.account.png");
            Bitmap image = new Bitmap(myStream);

            string accounts = System.Configuration.ConfigurationManager.AppSettings["account"].ToString();

            if(string.IsNullOrWhiteSpace(accounts))
            { 
                MessageBox.Show("No accounts set in the application config!", "NOTIFICATION", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }

            string[] arrAccount = accounts.Split(',');
            for (int i = 0; i < arrAccount.Length; i++)
            {
                ddbAccount.DropDownItems.Add(arrAccount[i], image);
            }
        }

        /// <summary>
        /// call the login service
        /// </summary>
        /// <param name="account">netsuite account</param>
        private void executeLogin(string account)
        {
            Login login = new Login();

            ///password not mandatory in configuration
            if (string.IsNullOrEmpty(login.Password))
            {
                FrmDialog passwordDialog = new FrmDialog();
                passwordDialog.Text = login.Email;
                if (passwordDialog.ShowDialog(this) == DialogResult.OK)
                {
                    login.Password = passwordDialog.txtPassword.Text;
                }
                else
                {
                    login.Password = string.Empty;
                }
                passwordDialog.Dispose();
            }


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
                appendLog(lblToolStripStatus.Text, NOTIFICATION_ERROR);
            }
        }

        /// <summary>
        /// start watching a specific path
        /// </summary>
        /// <param name="path"></param>
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

        /// <summary>
        /// stop watching all the active watchers
        /// </summary>
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

        /// <summary>
        /// load the image resources embedded in the assembly
        /// </summary>
        private void loadResources()
        {
            Stream myStream = NetsuiteUploaderAssembly.GetManifestResourceStream("NetsuiteUploader.Resources.done.png");
            Bitmap image = new Bitmap(myStream);
            picDone.Image = image;

            myStream = NetsuiteUploaderAssembly.GetManifestResourceStream("NetsuiteUploader.Resources.error.png");
            image = new Bitmap(myStream);
            picError.Image = image;

            myStream = NetsuiteUploaderAssembly.GetManifestResourceStream("NetsuiteUploader.Resources.folder.png");
            image = new Bitmap(myStream);
            mniTasksOpenFolder.Image = image;

            myStream = NetsuiteUploaderAssembly.GetManifestResourceStream("NetsuiteUploader.Resources.file.png");
            image = new Bitmap(myStream);
            mniTasksOpenFile.Image = image;
        }

        #endregion
    }
}
