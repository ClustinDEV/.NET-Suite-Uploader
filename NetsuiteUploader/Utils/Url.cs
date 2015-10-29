using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Runtime.Serialization.Json;
using System.Net;
using System.IO;
using System.Xml;
using System.Web.UI;

namespace NetsuiteUploader.Utils
{
    class Url
    {
        public Triplet getDataCenterUrls(String nsAccount, String nsEmail, String nsPassword)
        {
            try
            { 
                // Preset Values
                string sysURL = "https://rest.netsuite.com/rest/roles";
                Triplet urls = new Triplet();

                HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(sysURL);

                // 'nlauth_account' should NOT be specified for this type of request otherwise an error is returned.
                // Issue 238527
                httpWebRequest.Headers.Add("Authorization",
                                           "NLAuth nlauth_email=" + nsEmail + "," +
                                           "nlauth_signature=" + nsPassword);

                // set content type
                httpWebRequest.ContentType = "application/json";

                // execute http GET
                WebResponse webResponse = httpWebRequest.GetResponse();
                Stream httpBody = webResponse.GetResponseStream();

                string jsonContent = "";
                int i = 0;

                StreamReader streamReader = new StreamReader(httpBody);

                while (jsonContent != null)
                {
                    i++;
                    jsonContent = streamReader.ReadLine();
                    Stream mainContent = streamReader.BaseStream;

                    if (jsonContent != null)
                    {

                        // convert json content to bytes
                        byte[] bytesJsonContent = Encoding.Unicode.GetBytes(jsonContent);

                        // convert the json to an xml format so that xpath can be used to locate specific
                        // nodes & attributes
                        XmlDictionaryReader xmlReader = JsonReaderWriterFactory.CreateJsonReader(bytesJsonContent, new XmlDictionaryReaderQuotas());

                        // this returns the top-level element of the xml document;
                        // all of the following xpath statements are executed off of this element
                        XElement root = XElement.Load(xmlReader);

                        //Linq Query
                        IEnumerable<XElement> restDomains =
                        from c in root.Elements("item")
                        where (string)c.Element("account").Element("internalId").Value == nsAccount
                        select c;


                        // Outputs Data Center URLs for the specified account
                        if (restDomains != null)
                        {
                            foreach (XElement elementFromLinq in restDomains)
                            {
                                string accountIntId = elementFromLinq.Element("account").Element("internalId").Value;
                                string accountName = elementFromLinq.Element("account").Element("name").Value;
                                //Console.WriteLine("Data Center URLs for " + accountIntId + " " + accountName);
                                //Console.WriteLine("-----------------------");
                                //Console.WriteLine("Role: " + elementFromLinq.Element("role").Element("name").Value);
                                //Console.WriteLine("Rest Domain: " + elementFromLinq.Element("dataCenterURLs").Element("restDomain").Value);
                                //Console.WriteLine("System Domain: " + elementFromLinq.Element("dataCenterURLs").Element("systemDomain").Value);
                                //Console.WriteLine("Web Services Domain: " + elementFromLinq.Element("dataCenterURLs").Element("webservicesDomain").Value);
                                //Console.WriteLine(" ");
                                urls.First = elementFromLinq.Element("dataCenterURLs").Element("webservicesDomain").Value;   //webservicesDomain
                                urls.Second = elementFromLinq.Element("dataCenterURLs").Element("systemDomain").Value;  //systemDomain
                                urls.Third = elementFromLinq.Element("dataCenterURLs").Element("restDomain").Value;  //restDomain
                                break;
                            }
                        }

                        //Console.Write("Number of Roles for this Account: " + restDomains.Count());
                        //if (restDomains != null)
                        //{
                        //    Console.WriteLine("Roles");
                        //    Console.WriteLine("-----------------------");
                        //    foreach (XElement elementFromLinq in restDomains)
                        //    {
                        //        Console.WriteLine(elementFromLinq.Element("role").Element("name").Value);
                        //    }
                        //}
                    }
                }
                return urls;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
