using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model
{    
    public class OnlyOneFile
    {
        public IEnumerable<string> LocalFileNames { get; set; }
        public IEnumerable<string> RealFileNames { get; set; }
        public IEnumerable<string> ContentTypes { get; set; }

        public IEnumerable<string> Name { get; set; }
        public IEnumerable<string> FileNameStar { get; set; }

        public IEnumerable<long?> ContentLength { get; set; }
        public IEnumerable<long?> Size { get; set; }
        public IEnumerable<string> StrHeaders { get; set; }

        public string firm_name { get; set; }
    }

    #region RegistResult_Server
    public class RegistResult_Server
    {
        public int code { get; set; }
        public string message { get; set; }
        public RegistResult_Server_Data data = new RegistResult_Server_Data();
    }

    public class RegistResult_Server_Data
    {
    }
    #endregion
}
