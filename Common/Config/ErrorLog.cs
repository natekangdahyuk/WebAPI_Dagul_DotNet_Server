using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Common
{
    public class ErrLog
    {
        public static void SetErrorLogDBIns(string errMessage, string method, string param, string gameID)
        {
            SqlConnection con = new SqlConnection(ConnectionString.DB_CARGO);
            SqlCommand cmd = new SqlCommand("dbo.up_WebServiceErrorLog_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            int errMessagLen = errMessage.Length;
            if (errMessagLen > 800)
            {
                errMessage = errMessage.Substring(0, 800);
            }
            cmd.Parameters.Add("@errMessage", SqlDbType.NVarChar, 800).Value = errMessage;
            cmd.Parameters.Add("@method", SqlDbType.VarChar, 100).Value = method;
            cmd.Parameters.Add("@Param", SqlDbType.NVarChar, 800).Value = param;
            cmd.Parameters.Add("@GameID", SqlDbType.VarChar, 50).Value = gameID;

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
