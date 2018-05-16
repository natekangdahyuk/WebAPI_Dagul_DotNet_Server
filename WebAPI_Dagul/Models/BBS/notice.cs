using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.BBS
{
    public class notice_list_client
    {
        public int group_no { get; set; }
        public int owner_no { get; set; }
    }

    public class notice_list_client_result
    {
        public string Status_Code { get; set; }
        public string Status_Msg { get; set; }
        
        public List<ResultData_notice_list> Result_Data { get; set; }
    }

    public class ResultData_notice_list
    {
        public int NOTICE_IDX { get; set; }//테스트 넘버
        public string NOTICE_TITLE { get; set; } //타이틀
    }
}