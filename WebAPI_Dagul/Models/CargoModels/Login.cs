using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.CargoModels
{
    public class Owner_Login_Client
    {
        public string phoneno { get; set; }
        public string passwd { get; set; }
    }

    public class ResultData
    {
        public string BIZ_LICENSE { get; set; }
        public string DUTY_POSITION { get; set; }
        public string PASSWD { get; set; }
        public int GROUP_NO { get; set; }
        public string FIRM_REPRESENT { get; set; }
        public string SESSION_ID { get; set; }
        public string CONFIRM_DT { get; set; }
        public string SERVICE_TERM { get; set; }
        public string MOD_DT { get; set; }
        public int OWNER_NO { get; set; }
        public string USE_YN { get; set; }
        public string CONFIRM_YN { get; set; }
        public string REG_DT { get; set; }
        public string CONFIRM_CODUSER { get; set; }
        public string WORK_POSITION { get; set; }
        public string REGIGN_DT { get; set; }
        public string USERNM { get; set; }
        public string MEMO { get; set; }
        public string PHONE_NO { get; set; }
        public string WORK_STATUS { get; set; }
        public string TRANSPORT_LICENSE { get; set; }
        public string EMAIL { get; set; }
        public string BRANCH_OFFICE { get; set; }
        public string PERSONAL_INFO_PROVIDER_TERM { get; set; }
        public int GRADE { get; set; }
        public string BIZ_NO { get; set; }
        public string FIRM_NAME { get; set; }
        public string LOCATION_TERM { get; set; }
        public string LAST_LOGIN_DT { get; set; }
        public string PERSONAL_INFO_TERM { get; set; }
    }

    public class Owner_Login_Result
    {
        public string Status_Code { get; set; }
        public string Status_Msg { get; set; }
        public List<ResultData> Result_Data { get; set; }
    }
}