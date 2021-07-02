using AplicationQR.Elasticsearch;
using AplicationQR.Models;
using IronOcr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AplicationQR.Controllers
{
    [Route("demo")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("document")]
        public IActionResult Document()
        {
            return View();
        }
        [Route("generate")]
        public IActionResult Generate(string qrtext)
        {
            ViewBag.qrtext = qrtext;
            return View("Index");
        }

        [Route("file")]
        public IActionResult File(string contentFile)
        {
            FileCreator.createPdfFileMethod(contentFile);
            return View("Index");
        }

        [Route("privacy")]
        public IActionResult Privacy()
        {
            return View();
        }
        [Route("ocr")]
        public IActionResult Ocr()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

         [HttpPost("OcrFile")]
        public async Task<IActionResult> OcrFile(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    // full path to file in temp location
                    var filePath = Path.GetTempFileName(); //we are using Temp file name just for the example. Add your own file path.
                    string path = Path.GetFullPath(formFile.FileName);
                   string rootPath = Path.GetPathRoot(formFile.FileName);
                    string folderName = Path.GetFullPath(formFile.FileName);
                    string folderPath = Directory.GetCurrentDirectory();
                    System.IO.File.Copy(formFile.FileName, folderPath, true);
                    //File.Copy(Path.Combine(folderPath, );
                    var Result = new IronTesseract().Read(path);
                    ViewBag.ocrData = Result.Text;
                }
            }

            //HttpPostedFileBase file = Request.Files[0 ];

            return View("Index");

        }

    }
}
