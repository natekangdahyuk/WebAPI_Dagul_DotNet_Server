using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Common.DB
{
    public class Driver
    {
        #region 드라이버 기본 정보를 DB 에 입력한다.- 이미지 포함
        public static DataTable RegistDriverWithFiles(Model.OnlyOneFile retistDriver)
        {
            DataTable ListData = new DataTable();
            try
            {
                SqlConnection con = new SqlConnection(ConnectionString.DB_CARGO);
                SqlCommand com = new SqlCommand("USP_DRIVER_REG", con);
                com.CommandType = CommandType.StoredProcedure;
                                
                com.Parameters.Add("@UPDATOR", SqlDbType.VarChar, 30).Value = "웹어서로그인안한관리자"; //나중에는 어드민 툴에서 받아서


                SqlDataAdapter Adapter = new SqlDataAdapter(com);
                Adapter.Fill(ListData);

                return ListData;
            }
            catch (Exception ex)
            {
                //날짜용 파일에 떨구면 너무 부담이 되려나?
                string method = "Driver.RegistDriverWithFiles";
                //string param = "userID=" + userID + " prod_id=" + prod_id;
                string param = "too long";
                string projectName = Common.Global.ProjectName;
                Common.ErrLog.SetErrorLogDBIns(ex.Message.ToString(), method, param, projectName);

            }
            return null;
        }
        #endregion

    }
}
