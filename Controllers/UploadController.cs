using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using System.Net;
using System;
using Microsoft.AspNetCore.Hosting;
using SignalrServer.Extensions;
using SignalrServer.Hubs;

namespace SignalrServer.Controllers
{
    [Route("api/upload")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private const string ChannelName = "test-channel";
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly HttpResponseMessage _badRequest = new(HttpStatusCode.BadRequest);

        public UploadController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public HttpResponseMessage Post(object obj)
        {
            var httpRequest = HttpContext.Request;
            var files = httpRequest.Form.Files;
            if (files.Count <= 0) return this._badRequest;

            var httpPostedFile = files["UploadedImage"];
            if (httpPostedFile == null) return this._badRequest;

            // SAVE FILE ON DISK
            var extension = Path.GetExtension(httpPostedFile.FileName);
            var guid = Guid.NewGuid();
            var uploadFileName = $"{@"~/Uploads/"}{guid}{extension}";

            var fileSavePath = Path.Combine(this._webHostEnvironment.WebRootPath, uploadFileName);
            //var fileSavePath = HttpContext.Server.MapPath($"{@"~/Uploads/"}{guid}{extension}");

            httpPostedFile.SaveAs(fileSavePath); // EXTENSION !!!

            //// PUBLISH THAT NEW FILE IS READY TO BE UPLOADED
            //MyHub.SendMessage(ChannelName, uploadFileName);

            return new HttpResponseMessage(HttpStatusCode.Created);
        }
    }
}
