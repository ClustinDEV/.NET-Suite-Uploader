using NetsuiteUploader.com.netsuite.na1.webservices;
using System;

namespace NetsuiteUploader.Utils
{
    public class DataCenterAwareNetSuiteService : NetSuiteService
    {
        public DataCenterAwareNetSuiteService(string account, bool sandbox)
            : base()
        {
            if(sandbox)
            {
                Uri dataCenterUri = new Uri("https://webservices.sandbox.netsuite.com/services/NetSuitePort_2014_2");
                this.Url = dataCenterUri.ToString();
            }
            else
            { 
                System.Uri originalUri = new System.Uri(this.Url);
                DataCenterUrls urls = getDataCenterUrls(account).dataCenterUrls;
                Uri dataCenterUri = new Uri(urls.webservicesDomain + originalUri.PathAndQuery);
                this.Url = dataCenterUri.ToString();
            }
        }
    }
}