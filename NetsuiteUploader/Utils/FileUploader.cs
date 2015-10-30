using NetsuiteUploader.com.netsuite.na1.webservices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetsuiteUploader.Utils
{
    /// <summary>
    /// manage the upload of files on netsuite
    /// </summary>
    public static class FileUploader
    {
        /// <summary>
        /// exceute the upload of files set in the task
        /// </summary>
        /// <param name="netSuiteService">netsuite webservice instance</param>
        /// <param name="taskName">task executed</param>
        /// <returns>array of task files uploaded</returns>
        public static TaskFile[] UploadFiles(NetSuiteService netSuiteService, string taskName)
        {
            /*  ///SINGLE FILE SAMPLE UPLOAD
            RecordRef recRef = new RecordRef();
            recRef.internalId = "8700";
            file.folder = recRef;

            WriteResponse wr = netSuiteService.add(file);
            lblNotification.Text = wr.status.isSuccess.ToString();
            */

            string filePath = ConfigurationManager.AppSettings["taskfolder"].ToString().TrimEnd('\\') + @"\" + taskName;

            string taskContent = System.IO.File.ReadAllText(filePath);

            List<TaskFile> taskFiles = JsonConvert.DeserializeObject<TaskFile[]>(taskContent).ToList();
            List<TaskFile> taskFilesFolders = new List<TaskFile>();
            List<Record> records = new List<Record>(taskFiles.Count);

            for (int i = 0; i < taskFiles.Count; i++)
            {
                if(System.IO.Directory.Exists(taskFiles[i].Path))
                { ///loop for all files in folder
                    string[] files = System.IO.Directory.GetFiles(taskFiles[i].Path);
                    for (int k = 0; k < files.Length; k++)
                    {
                        TaskFile taskFile = new TaskFile() { Path = files[k], Folderid = taskFiles[i].Folderid };
                        taskFilesFolders.Add(taskFile);
                        records.Add(createRecord(taskFile));
                    }
                }
                else
                { ///single file
                    records.Add(createRecord(taskFiles[i]));
                }
            }

            netSuiteService.addListAsync(records.ToArray());

            taskFiles.AddRange(taskFilesFolders);

            return taskFiles.ToArray();
        }

        /// <summary>
        /// create the nesuite record of file for the upload
        /// </summary>
        /// <param name="taskFile">task file</param>
        /// <returns>netsuite file record</returns>
        private static Record createRecord(TaskFile taskFile)
        {
            string path = taskFile.Path;
            if (!System.IO.File.Exists(path))
                throw new Exception("Error in Task: file does not exist [" + path + "]");

            string folderInternalId = taskFile.Folderid;
            NetsuiteUploader.com.netsuite.na1.webservices.File record = new NetsuiteUploader.com.netsuite.na1.webservices.File();
            record.name = System.IO.Path.GetFileName(path);
            record.content = System.IO.File.ReadAllBytes(path);
            RecordRef recRef = new RecordRef();
            recRef.internalId = folderInternalId;
            record.folder = recRef;

            return record;
        }

    }
}
