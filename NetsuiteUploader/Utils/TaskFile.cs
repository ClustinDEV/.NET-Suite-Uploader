using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetsuiteUploader.Utils
{
    /// <summary>
    /// task file struct
    /// </summary>
    public class TaskFile
    {
        /// <summary>
        /// path of the local file
        /// </summary>
        public string Path;

        /// <summary>
        /// netsuite cabinet folder id where the file will be upload
        /// </summary>
        public string Folderid;
    }
}
