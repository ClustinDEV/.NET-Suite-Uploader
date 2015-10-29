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
        public string Account = null;
        public string Email = ConfigurationManager.AppSettings["email"];
        public string Password = ConfigurationManager.AppSettings["password"];

        public SessionResponse login(NetSuiteService netSuiteService)
        {
            return login(netSuiteService, null);
        }

        public SessionResponse login(NetSuiteService netSuiteService, string account)
        {
            try
            {
                this.Account = account;
                if (this.Account == null)
                    this.Account = ConfigurationManager.AppSettings["account"].Split(',')[0];

                Passport passport = new Passport();
                passport.account = this.Account;
                passport.email = this.Email;
                passport.password = this.Password;
                //RecordRef role = new RecordRef();
                //role.internalId = "3";
                //passport.role = role;

                netSuiteService.CookieContainer = new CookieContainer();
                //netSuiteService.Url = "https://webservices.na1.netsuite.com/services/NetSuitePort_2014_2";

                SessionResponse sessionResponse = netSuiteService.login(passport);
                return sessionResponse;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
        }

        public void logout(NetSuiteService netSuiteService, SessionResponse sessionResponse)
        {
            if (sessionResponse != null && sessionResponse.status.isSuccess)
                netSuiteService.logout();
        }
    }
}
