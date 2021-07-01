using AplicationQR.Elasticsearch;
using AplicationQR.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicationQR.Controllers
{
    public class DocumentController : Controller
    {
        public static ElasticClient client = null;
        public IActionResult Index()
        {
           client = ElasticClientSetUp.getClient("document");
            ViewBag.ElkClient = client;
            return View();
        }
        [HttpPost]
        public IActionResult Search(string searchString, bool notUsed)
        {

            List<Document> list = ElasticClientSetUp.searchIndexWithCriteria(client,"document",searchString);
            ViewBag.ListDocument = list;

            return View("Index");
            
        }
        [HttpPost]
        public IActionResult Upload(string name, string content, bool notUsed)
        {


            Document document = new Document
            {
                creatorId = 5,
                dateCreated = DateTime.Now,
                fileName = name,
                fileContent = content,
                dateModifier = null
            };

            ElasticClientSetUp.InsertDocument(client, document);


            return View("Index");

        }

        public IActionResult Create()
        {

            return View();
        }
    }
}
