using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using AgileEAP.Core;

namespace AgileEAP.Web
{
    public class UploadController : ApiController
    {
        [HttpPost]
        //[ActionName("ProcessDefine")]
        public async Task<HttpResponseMessage> PostFile()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string installPath = HttpContext.Current.Server.MapPath("~/TempFileDirectory/Plugins");
            var provider = new MultipartFormDataStreamProvider(installPath);

            try
            {
                StringBuilder sb = new StringBuilder(); // Holds the response body

                // Read the form data and return an async task.
                await Request.Content.ReadAsMultipartAsync(provider);

                // This illustrates how to get the form data.
                foreach (var key in provider.FormData.AllKeys)
                {
                    foreach (var val in provider.FormData.GetValues(key))
                    {
                        sb.Append(string.Format("{0}: {1}\n", key, val));
                    }
                }

                // This illustrates how to get the file names for uploaded files.
                foreach (var file in provider.FileData)
                {
                    FileInfo fileInfo = new FileInfo(file.LocalFileName);
                    sb.Append(string.Format("Uploaded file: {0} ({1} bytes)\n", fileInfo.Name, fileInfo.Length));
                }
                //return new HttpResponseMessage()
                //{
                //    Content = new StringContent(sb.ToString())
                //};


                string thumbnailPath = HttpContext.Current.Server.MapPath("~/Content/Images/plugin.png");
                if (!Directory.Exists(installPath)) Directory.CreateDirectory(installPath);
                var statuses = new List<ViewDataUploadFilesResult>();
                foreach (var file in provider.FileData)
                {
                    //var file = Request.Files[i];
                    //var fullPath = Path.Combine(installPath, Path.GetFileName(file.FileName));
                    //file.SaveAs(fullPath);

                    statuses.Add(new ViewDataUploadFilesResult()
                    {
                        name = file.LocalFileName,
                        size = file.Headers.ContentLength,
                        type = file.Headers.ContentType.MediaType,
                        url = "/Plugin/Download/" + file.LocalFileName,
                        delete_url = "/Plugin/Delete/" + file.LocalFileName,
                        thumbnail_url = @"data:image/png;base64," + Convert.ToBase64String(System.IO.File.ReadAllBytes(thumbnailPath)),
                        delete_type = "GET",
                    });
                }
                return new HttpResponseMessage()
                {
                    Content = new StringContent(JsonConvert.SerializeObject(statuses))
                };
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
         

        public class ViewDataUploadFilesResult
        {
            public string name { get; set; }
            public long? size { get; set; }
            public string type { get; set; }
            public string url { get; set; }
            public string delete_url { get; set; }
            public string thumbnail_url { get; set; }
            public string delete_type { get; set; }
        }
    }
}