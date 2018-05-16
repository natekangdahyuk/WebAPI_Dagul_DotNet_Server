using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Threading.Tasks;
using System.IO;
using System.Web.Hosting;
using Newtonsoft.Json;
using System.Data;


namespace WebAPI_Dagul.Controllers
{
    [RoutePrefix("api/FileUpload")]
    public class FileUploadController : ApiController
    {  

        [Route("regist_DataWithSingleFiles")]
        [HttpPost]        
        public async Task<string> regist_DataWithSingleFiles()
        {
            Common.Model.RegistResult_Server serverMsg = new Common.Model.RegistResult_Server();

            string stWriteData;

            //미디어타입인지 검사
            if (!Request.Content.IsMimeMultipartContent())
            {
                serverMsg.code = -9999;
                serverMsg.message = "UnsupportedMediaType";

                stWriteData = Common.Lib.cJSON._Serialize(serverMsg);
                return stWriteData;
            }

            //전달된 스트림을 임시 폴더에 저장시킨다음...읽어서
            var streamProvider = new MultipartFormDataStreamProvider(Common.Global.FileUploadPath);
            await Request.Content.ReadAsMultipartAsync(streamProvider);

            Common.Model.OnlyOneFile RecvPostData = new Common.Model.OnlyOneFile();

            RecvPostData.LocalFileNames = streamProvider.FileData.Select(entry => entry.LocalFileName); //"D:\\wwwRoot\\uploadFiles\\codLabs\\BodyPart_c57402a3-9ef2-4b76-8e85-a764f6631d87"
            RecvPostData.RealFileNames = streamProvider.FileData.Select(entry => entry.Headers.ContentDisposition.FileName);//"\"xxx.png\"" <- Replace필요
            RecvPostData.ContentTypes = streamProvider.FileData.Select(entry => entry.Headers.ContentType.MediaType);//"image/jpeg"


            RecvPostData.Name = streamProvider.FileData.Select(entry => entry.Headers.ContentDisposition.Name); // "\"fileUpload\"" <- Replace 필요            
            RecvPostData.Size = streamProvider.FileData.Select(entry => entry.Headers.ContentDisposition.Size);
            RecvPostData.StrHeaders = streamProvider.FileData.Select(entry => entry.Headers.ContentDisposition.ToString());

            RecvPostData.firm_name = streamProvider.FormData["firm_name"];

            //파일먼저 저장하고 에러가 날경우 파일 삭제
            //하드코딩으로 순서대로 파일이 올라왔는지 확인해서 저장
            string RealFileName = string.Empty;
            string LocalFileNames = string.Empty;
            string ContentTypes = string.Empty;
            string Name = string.Empty;


            for (int i = 0; i < RecvPostData.RealFileNames.Count(); i++)
            {
                RealFileName = RecvPostData.RealFileNames.ElementAt(i).Replace("\"", "");
                LocalFileNames = RecvPostData.LocalFileNames.ElementAt(i).Replace("\"", "");
                ContentTypes = RecvPostData.ContentTypes.ElementAt(i).Replace("\"", ""); ////"image/jpeg", "image/png", "image/gif"
                Name = RecvPostData.Name.ElementAt(i).Replace("\"", ""); //input type의 이름            

                if (ContentTypes == "image/jpeg" || ContentTypes == "image/png" || ContentTypes == "image/gif" || ContentTypes == "application/pdf")
                {
                    string lastFileName;
                    string FullPathFileName = Common.Lib.fileUpload.GetUniqueFileNameWithPath(Common.Global.FileUploadPath, RealFileName, out lastFileName);
                    
                    //나중을 위해 그냥 파일 사이즈를 알아오자
                    long filesize = Common.Lib.fileUpload.GetFileSize(LocalFileNames);
                    //if(filesize > 30000000000) // 30메가가 넘는 파일을 첨부했다면.. 뭐 이딴식의 예외처리가 가능 할듯..

                    //DB 에 저장될 이름 세팅이 끝났다면 로컬템프 파일을 찾아서 해당 파일명으로 바꿔주고 와라.
                    string ret_renameFile = Common.Lib.fileUpload.ReNameFile(Common.Global.FileUploadPath, LocalFileNames, lastFileName);
                    if (ret_renameFile != "success")
                    {
                        serverMsg.code = -9999;
                        serverMsg.message = "File Name Fail";

                        stWriteData = Common.Lib.cJSON._Serialize(serverMsg);
                        return stWriteData;
                    }
                }
                else //템프파일을 지운다.
                {
                    Common.Lib.fileUpload.DeleteTempFile(LocalFileNames);
                }
            }//모든 파일 저장 완료

            //////////////////////////
            //////////////////////////
            //////////////////////////
            //////////////////////////
            //임시로 만들자 끝
            serverMsg.code = 0;
            serverMsg.message = "success";

            stWriteData = Common.Lib.cJSON._Serialize(serverMsg);
            return stWriteData;
            //임시로 만들자 끝
            //////////////////////////
            //////////////////////////
            //////////////////////////
            //////////////////////////

            

            //파일 업로드 & Post 데이타 저장이 완료
        }

        
    }
}