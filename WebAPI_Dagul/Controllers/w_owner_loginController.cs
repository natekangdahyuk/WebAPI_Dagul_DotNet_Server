using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebAPI_Dagul.Controllers
{    
    public class w_owner_loginController : ApiController
    {
        [Route("w_owner_login")] //화주 웹 로그인
        [HttpPost]
        public string owner_login(Models.CargoModels.Owner_Login_Client loginClient)
        {
            Models.CargoModels.Owner_Login_Result serverMsg = new Models.CargoModels.Owner_Login_Result();

            // 테스트 이기에.. 그냥.. 임의의 값을 만들어 리턴 시킨다.
            string stWriteData = "{ \"Status_Code\":\"200\", \"Status_Msg\":\"Successfuly\",\"Result_Data\" : [{\"BIZ_LICENSE\": \"123-45-67890\", \"DUTY_POSITION\": null, \"GROUP_NO\": 1, \"FIRM_REPRESENT\": \"대표자명\", \"SESSION_ID\": \"926F84D5-D3C4-49E3-BDEF-ECC7351206BA\", \"CONFIRM_DT\": \"2017-09-07T12:20:36.337000\", \"SERVICE_TERM\": null, \"MOD_DT\": null, \"OWNER_NO\": 1, \"USE_YN\":\"Y\", \"CONFIRM_YN\": \"Y\", \"REG_DT\": \"2017-09-07T12:20:36.337000\", \"CONFIRM_CODUSER\": null, \"WORK_POSITION\": null, \"REGIGN_DT\": null, \"USERNM\": \"김종열\", \"MEMO\": null, \"PHONE_NO\": \"010-3240-2703\", \"WORK_STATUS\": null, \"TRANSPORT_LICENSE\": null, \"EMAIL\": \"jongyoul.kim@codlabs.com\", \"BRANCH_OFFICE\": null, \"PERSONAL_INFO_PROVIDER_TERM\": null, \"GRADE\": 1, \"BIZ_NO\": null, \"FIRM_NAME\": \"법인씨오디\", \"LOCATION_TERM\": null, \"LAST_LOGIN_DT\": \"2017-09-08T12:05:41.590000\", \"PERSONAL_INFO_TERM\": null }]}";
            return stWriteData;
        }

        [Route("w_owner_login_Get")] //화주 웹 로그인-겟 방식
        [HttpGet]
        public string owner_login_Get(string phoneno, string passwd)
        {
            // 테스트 이기에.. 그냥.. 임의의 값을 만들어 리턴 시킨다.
            string stWriteData = "{ \"Status_Code\":\"200\", \"Status_Msg\":\"Successfuly\",\"Result_Data\" : [{\"BIZ_LICENSE\": \"123-45-67890\", \"DUTY_POSITION\": null, \"GROUP_NO\": 1, \"FIRM_REPRESENT\": \"대표자명\", \"SESSION_ID\": \"926F84D5-D3C4-49E3-BDEF-ECC7351206BA\", \"CONFIRM_DT\": \"2017-09-07T12:20:36.337000\", \"SERVICE_TERM\": null, \"MOD_DT\": null, \"OWNER_NO\": 1, \"USE_YN\":\"Y\", \"CONFIRM_YN\": \"Y\", \"REG_DT\": \"2017-09-07T12:20:36.337000\", \"CONFIRM_CODUSER\": null, \"WORK_POSITION\": null, \"REGIGN_DT\": null, \"USERNM\": \"김종열\", \"MEMO\": null, \"PHONE_NO\": \"010-3240-2703\", \"WORK_STATUS\": null, \"TRANSPORT_LICENSE\": null, \"EMAIL\": \"jongyoul.kim@codlabs.com\", \"BRANCH_OFFICE\": null, \"PERSONAL_INFO_PROVIDER_TERM\": null, \"GRADE\": 1, \"BIZ_NO\": null, \"FIRM_NAME\": \"법인씨오디\", \"LOCATION_TERM\": null, \"LAST_LOGIN_DT\": \"2017-09-08T12:05:41.590000\", \"PERSONAL_INFO_TERM\": null }]}";
            
            return stWriteData;
        }
    }
}
