//=============================================================================
// Description
// 		로그 파일 
//		LOG_BUFFER_SIZE(10K) 만큼 버퍼링을 사용하고
//		LOG_FILE_SIZE(4M) 마다 새 파일 생성
//=============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ServerAFC
{
    public class cLogFile
    {
        public cLogFile()
        {
            sbBuffer = new StringBuilder(LOG_BUFFER_SIZE);
            nIterator = 0;
            bBuffering = true;
        }

        ~cLogFile()
        {
            Close();
        }

        #region PUBLIC FUNCTION
        //=============================================================================
        // [mirime 2011.08.11]
        // Description : 파일 이름을 받아서 파일을 쓸 준비를 한다.
        //=============================================================================
        public bool Open(string strPath, bool bBuffering = true)
        {
            if (strPath == null || strPath.Length == 0)
                return false;

            lock (oLock)
            {
                if (IsOpen())
                    return false;

                this.strPath = strPath;
                this.dtStart = DateTime.Now;
                this.bBuffering = bBuffering;

                MakeDirectory();
                if (!OpenFile())
                    return false;
            }

            return true;
        }

        //=============================================================================
        // [mirime 2011.08.11]
        // Description : 버퍼의 내용을 파일에 적고 파일을 닫는다.
        //=============================================================================
        public void Close()
        {
            lock (oLock)
            {
                if (IsOpen())
                {
                    FlushFile();
                    CloseFile();
                }
            }
        }

        //=============================================================================
        // [mirime 2011.08.11]
        // Description : 버퍼의 내용을 파일에 쓴다.
        //=============================================================================
        public void Flush()
        {
            lock (oLock)
            {
                if (IsOpen())
                {
                    FlushFile();
                }
            }
        }

        //=============================================================================
        // [mirime 2011.08.11]
        // Description : 한줄 파일을 버퍼에 쓴다.
        //=============================================================================
        public bool WriteLine(string strMsg)
        {
            lock (oLock)
            {
                if (!IsOpen())
                    return false;

                // 버퍼에 추가 한다.
                sbBuffer.Append(strMsg);
                if (!bBuffering || sbBuffer.Length > LOG_BUFFER_SIZE)
                {
                    // 버퍼를 길이를 검사해서 파일에 쓰도록 한다.
                    FlushFile();
                    if (swFile.BaseStream.Position >= LOG_FILE_SIZE)
                    {
                        // 파일 사이즈를 검사해서 새로운 파일을 만든다.
                        CloseFile();

                        if (!OpenFile())
                            return false;
                    }
                }
            }

            return true;
        }
        #endregion

        #region PRIVATE FUNCTION
        //=============================================================================
        // [mirime 2011.08.11]
        // Description : 오픈 상태를 확인
        //=============================================================================
        private bool IsOpen()
        {
            return (swFile != null);
        }

        //=============================================================================
        // [mirime 2011.08.11]
        // Description : 시간과 인덱스 번호로 파일을 생성한다. 
        //=============================================================================
        private bool OpenFile()
        {
            nIterator++;
            string strNextFilePath = String.Format("{0}\\{1}_{2}_{3:0000}{4}",
                Path.GetDirectoryName(strPath),
                Path.GetFileNameWithoutExtension(strPath),
                dtStart.ToString("yyyy-MM-dd HH_mm_ss"),
                nIterator,
                Path.GetExtension(strPath));
            swFile = new StreamWriter(new FileStream(strNextFilePath, FileMode.CreateNew, FileAccess.Write, FileShare.Read));
            if (swFile == null)
                return false;

            return true;
        }

        //=============================================================================
        // [mirime 2011.08.11]
        // Description : 파일을 닫는다.
        //=============================================================================
        private void CloseFile()
        {
            if (swFile != null)
            {
                swFile.Close();
                swFile.Dispose();
                swFile = null;
            }
        }

        //=============================================================================
        // [mirime 2011.08.11]
        // Description : 버퍼의 내용을 파일에  저장한다.
        //=============================================================================
        private void FlushFile()
        {
            if (sbBuffer.Length > 0)
            {
                swFile.Write(sbBuffer.ToString());
                sbBuffer.Length = 0;
            }

            swFile.Flush();
        }

        //=============================================================================
        // [mirime 2011.08.11]
        // Description : 경로의 폴더가 없으면 생성한다.
        //=============================================================================
        private void MakeDirectory()
        {
            string[] strDirs = Path.GetDirectoryName(strPath).Split('\\');
            string strDir = "";
            string strDelim = "";

            for (int i = 0; i < strDirs.Length; ++i)
            {
                strDir = strDir + strDelim + strDirs[i];
                if (strDir.Length > 0 && !Directory.Exists(strDir))
                {
                    Directory.CreateDirectory(strDir);
                }
                strDelim = "\\";
            }
        }
        #endregion

        #region VARIABE
        private const int LOG_BUFFER_SIZE = 10 * (1 << 10);	// 10K buffer
        private const int LOG_FILE_SIZE = 4 * (1 << 20);	// 4M rollover

        private string strPath;			// 파일 경로
        private DateTime dtStart;		// 첫번째 파일 생성 시간
        private int nIterator;			// 파일 일련 번호
        private StreamWriter swFile;
        private StringBuilder sbBuffer;
        private bool bBuffering;
        private object oLock = new object();
        #endregion
    }
}
