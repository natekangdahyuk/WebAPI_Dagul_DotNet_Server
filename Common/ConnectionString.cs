using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class ConnectionString
    {
        /* 샘플 시작
        public static string gameServerURL { get; set; }
        public static string fb_callBackUrl { get; set; }
        public static string Login_HomeUrl { get; set; }
        public static string fb_appID { get; set; }
        public static string fb_AppSecret { get; set; }
        public static string google_ClientId { get; set; }
        public static string google_ClientSecret { get; set; }
        public static string google_RedirectUri { get; set; }
         샘플 끝
        */

        public static string DB_CARGO { get; set; }
        public static string DB_CRAWL { get; set; }
        public static string DB_ErrorLog { get; set; }


        static ConnectionString()
        {
            switch (Global.Domain)
            {
                case Global.DomainEnum.Local:
                    DB_CARGO = "User ID=sa;Password=cod!2017;Persist Security Info=True;Initial Catalog=CARGO;Data Source=220.120.69.223,1434";
                    DB_CRAWL = "User ID=sa;Password=cod!2017;Persist Security Info=True;Initial Catalog=crawl;Data Source=220.120.69.223,1434";
                    DB_ErrorLog = "User ID=sa;Password=cod!2017;Persist Security Info=True;Initial Catalog=WebServiceLog;Data Source=220.120.69.223,1434";
                    //Driver_Post = "http://localhost:50024/api/Driver";
                    //fb_appID = "272363866519314";
                    //fb_AppSecret = "210acfd544a4e507ac26225bd7feb0c4";
                    //fb_callBackUrl = "http://localhost:3004/pc/login/fb_login_ok.aspx";
                    //google_ClientId = "196520409851-lfh4ksvvb7kngifccicit3v3nqq0fmdn.apps.googleusercontent.com";
                    //google_ClientSecret = "9HXRsaHnbBovSzDFc5bHwCW9";
                    //google_RedirectUri = "http://localhost:3004/pc/login/google_login_ok.aspx";
                    break;
                case Global.DomainEnum.Dev:
                    DB_CARGO = "User ID=sa;Password=cod!2017;Persist Security Info=True;Initial Catalog=CARGO;Data Source=220.120.69.223,1434";
                    DB_CRAWL = "User ID=sa;Password=cod!2017;Persist Security Info=True;Initial Catalog=crawl;Data Source=220.120.69.223,1434";
                    DB_ErrorLog = "User ID=sa;Password=cod!2017;Persist Security Info=True;Initial Catalog=WebServiceLog;Data Source=220.120.69.223,1434";
                    break;
                default: //real
                    DB_CARGO = "User ID=sa;Password=cod!2017;Persist Security Info=True;Initial Catalog=CARGO;Data Source=220.120.69.223,1434";
                    DB_CRAWL = "User ID=sa;Password=cod!2017;Persist Security Info=True;Initial Catalog=crawl;Data Source=220.120.69.223,1434";
                    DB_ErrorLog = "User ID=sa;Password=cod!2017;Persist Security Info=True;Initial Catalog=WebServiceLog;Data Source=220.120.69.223,1434";
                    break;
            }
        }
    }
}
