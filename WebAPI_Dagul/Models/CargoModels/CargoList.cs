using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models.CargoModels
{
    public class Owner_Cargo_List_Client
    {
        public int group_no { get; set; }
        public int owner_no { get; set; }
        public string session_id { get; set; }
        public int cargo_idx { get; set; } //마지막 화물 리스트 번호
        public string qty { get; set; }//페이지 당 몇개?
        public string col { get; set; } //검색 컬럼
        public string val { get; set; } //검색어
    }



    public class ResultData_CargoList
    {
        public int CARGO_IDX { get; set; }//오더넘버
        public string FIX_YN { get; set; } //운송료 형태_이놈
        public int STATE_TYPE { get; set; }//배차상태_이놈

        public string UP_PLACE_NAME { get; set; }//상차지명
        public string UP_ADDR1 { get; set; }//상차지주소1
        public string UP_ADDR2 { get; set; }//상차지주소2

        public string DOWN_PLACE_NAME { get; set; }//하차지명
        public string DOWN_ADDR1 { get; set; }//하차지주소1
        public string DOWN_ADDR2 { get; set; }//하차지주소2

        public int CARGO_TYPE { get; set; } //화물품목_이놈

        public int CAR_FEATURE { get; set; } //차량종류_이놈
        public string TONAGE { get; set; } //차량톤수_이놈

        //확정의 경우 - 화주의 확정 운송료, 차주의 제안금액, 차주의 제안시간
        public int PRICE { get; set; }
        public int REQUEST_PRICE { get; set; }
        public DateTime REQUEST_Date { get; set; }


        //변동의 경우 - 변동시작가, 변동현재가 (뷰에서는.. 시작시간부터. 현재 시간까지 얼마씩 올랐는지 계산해서 --max_price가 넘었으면 max, 아니면 현재가를 보여줌
        public DateTime START_DT { get; set; } //시작시간
        public int MIN_PRICE { get; set; } //시작가

        public DateTime END_DT { get; set; }//변동 종료시간
        public int MAX_PRICE { get; set; }//변동 최고가
        public int ADD_PRICE { get; set; } //변동 금액 단위
        public int ADD_MINUTE { get; set; } //변동 시간 단위


        public int PRICE_TYPE { get; set; } // 운송료지급종류_이놈

        //리스트에는 안들어 감
        public string UP_CONTACT_MAN { get; set; } //상차_담당자명
        public string UP_CONTACT_PHONE { get; set; } //상차 담당자 연락처
        public string UP_MEMO { get; set; } //상차 특이사항
        public int UP_TYPE { get; set; } //상차방법_이놈
        public double UP_XPOS { get; set; } //상차지 X 좌표값
        public double UP_YPOS { get; set; } //상차지 Y 좌표
        public DateTime UP_DATETIME { get; set; } //상차일시

        public string DOWN_CONTACT_MAN { get; set; } //하차 담당자 명
        public string DOWN_CONTACT_PHONE { get; set; }//하차 담당자 연락처
        public string DOWN_MEMO { get; set; }//하차특이사항
        public int DOWN_TYPE { get; set; } //하차방법_이놈
        public double DOWN_X { get; set; } //하차지 X좌표값
        public double DOWN_Y { get; set; } //하차지 Y 좌표값
        public DateTime DOWN_DATETIME { get; set; } //하차일시

        public string CARGO_DTL { get; set; } //화물에 대한 상세정보
        public string CARGO_MEMO { get; set; } //화물품목 특이사항 (길게 입력하는거)
        
        

        public string MIX_YN { get; set; } //혼적여부
        public string SHUTTLE_YN { get; set; } //왕복여부
        public int CAR_TYPE { get; set; } //차량종류 이놈


        public int OWNER_NO { get; set; } // 운영자 넘버
        public int GROUP_NO { get; set; } //그룹넘버

        public DateTime MOD_DT { get; set; } //수정 일시 (서버정의)
        public DateTime REG_DT { get; set; } //등록일시(서버정의)

        public DateTime CONFIRM_DT { get; set; } //체결시간 : 
        public int CONFIRM_PRICE { get; set; } //체결 금액

    }

    public class Owner_CargoList_Result
    {
        public string Status_Code { get; set; }
        public string Status_Msg { get; set; }
        public int TotalCnt { get; set; }
        public DateTime NOW_DT { get; set; } //서버의 현재시간 (변동에서 필요함) 

        public List<ResultData_CargoList> Result_Data { get; set; }
    }
}


