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
    public class FileUploader
    {
        public TaskFile[] UploadFiles(NetSuiteService netSuiteService, string taskName)
        {
            /*  ///SINGLE FILE SAMPLE UPLOAD
            RecordRef recRef = new RecordRef();
            recRef.internalId = "8700";
            file.folder = recRef;

            WriteResponse wr = netSuiteService.add(file);
            lblNotification.Text = wr.status.isSuccess.ToString();
            */

            string filePath = ConfigurationSettings.AppSettings["taskfolder"].ToString() + taskName;

            string taskContent = System.IO.File.ReadAllText(filePath);

            TaskFile[] files = JsonConvert.DeserializeObject<TaskFile[]>(taskContent);
            
            Record[] records = new Record[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                string path = files[i].Path;
                if (!System.IO.File.Exists(path))
                    throw new Exception("Error in Task: file does not exist [" + path + "]");
                
                string folderInternalId = files[i].Folderid;
                NetsuiteUploader.com.netsuite.na1.webservices.File record = new NetsuiteUploader.com.netsuite.na1.webservices.File();
                record.name = System.IO.Path.GetFileName(path);
                record.content = System.IO.File.ReadAllBytes(path);
                RecordRef recRef = new RecordRef();
                recRef.internalId = folderInternalId;
                record.folder = recRef;

                records[i] = record;
            }

            netSuiteService.addListAsync(records);

            return files;
        }

    }

    public class TaskFile
    {
        public string Path;
        public string Folderid;
    }

}
