using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System;
using SignalrServer.Extensions;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace SignalrServer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost]
        public JsonResult SaveFileOnDisk(IFormFile formFile)
        {
            if (formFile == null) return Json(new { status = "error" });
            try
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(formFile.FileName)}";
                var filePath = Path.Combine(
                    this._webHostEnvironment.WebRootPath, $"Uploads\\{fileName}");

                formFile.SaveAs(filePath); // EXTENSION !!!

                return Json(
                    new
                    {
                        status = "success",
                        fileName = formFile.FileName, 
                        fileSize = formFile.Length,
                        filePath = filePath
                    });
            }
            catch (Exception)
            {
                return Json(new { status = "error"  });
            }
        }
    }
}
