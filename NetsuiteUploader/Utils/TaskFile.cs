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

        /// <summary>
        /// file available without login
        /// </summary>
        public bool IsOnline = false;

        /// <summary>
        /// file is addable to bundle
        /// </summary>
        public bool Bundleable = false;

        /// <summary>
        /// file is hide in bundle
        /// </summary>
        public bool HideInBundle = false;

        /// <summary>
        /// file is inactive
        /// </summary>
        public bool IsInactive = false;

        /// <summary>
        /// file is private
        /// </summary>
        public bool IsPrivate = false;
    }
}
