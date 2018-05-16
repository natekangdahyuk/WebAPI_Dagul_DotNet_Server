using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ServerAFC
{
    public static class cLogger
    {
        #region ENUMERATION
        public enum ELevel
        {
            OFF = 0,
            ERROR = 1,
            WARNING = 2,
            INFO = 3,
            VERBOSE = 4
        }
        #endregion

        #region PUBLIC FUNCTION
        //=============================================================================
        // Description : 초기화. 초기화를 하지 않으면 파일로 저장하지 않는다.
        //		strPath : 파일 경로
        //		eLevel : 남길 로그 등급
        //=============================================================================
        public static void Initialize(string strPath, ELevel eLevel, bool bBuffering)
        {
            if (s_bInitialize)
            {
                return;
            }

            if (!s_logFile.Open(strPath, bBuffering))
            {
                return;
            }

            s_eLevel = eLevel;
            s_bInitialize = true;
        }

        public static void Close()
        {
            if (s_bInitialize)
            {
                s_logFile.Close();
                s_bInitialize = false;
            }
        }

        //=============================================================================
        // Description : Verbose 로그
        //=============================================================================
        public static void Verbose(string strFormat, params object[] oArgs)
        {
            string strMsg = String.Format(strFormat, oArgs);
            WriteLine(ELevel.VERBOSE, strMsg);
        }

        //=============================================================================
        // Description : Information 로그
        //=============================================================================
        public static void Information(string strFormat, params object[] oArgs)
        {
            string strMsg = String.Format(strFormat, oArgs);
            WriteLine(ELevel.INFO, strMsg);
        }

        //=============================================================================
        // Description : Warning 로그
        //=============================================================================
        public static void Warning(string strFormat, params object[] oArgs)
        {
            string strMsg = String.Format(strFormat, oArgs);
            WriteLine(ELevel.WARNING, strMsg);
        }

        //=============================================================================
        // Description : Error 로그
        //=============================================================================
        public static void Error(string strFormat, params object[] oArgs)
        {
            string strMsg = String.Format(strFormat, oArgs);
            WriteLine(ELevel.ERROR, strMsg);
        }
        #endregion

        #region PRIVATE FUNCTION
        //=============================================================================
        // Description : 등급을 검사해서 남기는 시간을 포함해서 로그를 남긴다.
        //=============================================================================
        private static void WriteLine(ELevel eLevel, string strMsg)
        {
            string strLog = String.Format("[{0}][{1,-7:G}]{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"), eLevel, strMsg);
            Debug.WriteLine(strLog);
            if ((s_bInitialize) && s_eLevel >= eLevel)
            {
                s_logFile.WriteLine(strLog);
            }
        }
        #endregion

        #region VARIABLE
        private static bool s_bInitialize = false;			// 초기화 상태 확인
        private static ELevel s_eLevel = ELevel.VERBOSE;	// 현재 레벨
        private static cLogFile s_logFile = new cLogFile();	// 로그 파일
        #endregion
    }
}
