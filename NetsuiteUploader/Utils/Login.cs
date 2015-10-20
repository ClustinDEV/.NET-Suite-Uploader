using NetsuiteUploader.com.netsuite.na1.webservices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace NetsuiteUploader.Utils
{
    public class Login
    {
        public string Account = ConfigurationSettings.AppSettings["account"];
        public string Email = ConfigurationSettings.AppSettings["email"];
        public string Password = ConfigurationSettings.AppSettings["password"];

        public SessionResponse login(NetSuiteService netSuiteService)
        {
            try
            {
                Passport passport = new Passport();
                passport.account = this.Account;
                passport.email = this.Email;
                passport.password = this.Password;
                //RecordRef role = new RecordRef();
                //role.internalId = "3";
                //passport.role = role;

                netSuiteService.CookieContainer = new CookieContainer();
                //netSuiteService.Url = "https://webservices.na1.netsuite.com";
                SessionResponse sessionResponse = netSuiteService.login(passport);
                return sessionResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
