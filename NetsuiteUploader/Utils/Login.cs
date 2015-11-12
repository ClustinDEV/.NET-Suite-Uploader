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
    /// <summary>
    /// login management
    /// </summary>
    public class Login
    {
        public string Account = null;
        public string Email = ConfigurationManager.AppSettings["email"];
        public string Password = ConfigurationManager.AppSettings["password"];

        public Login()
        {

        }

        public Login(string account)
        {
            this.Account = account;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="netSuiteService">netsuite webservice instance</param>
        /// <param name="account">account</param>
        /// <returns>session service response</returns>
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

        /// <summary>
        /// logout
        /// </summary>
        /// <param name="netSuiteService">netsuite webservice instance</param>
        /// <param name="sessionResponse">session service response</param>
        public void logout(NetSuiteService netSuiteService, SessionResponse sessionResponse)
        {
            if (sessionResponse != null && sessionResponse.status.isSuccess)
                netSuiteService.logout();
        }
    }
}
