using AplicationQR.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicationQR.Elasticsearch
{
    public class ElasticClientSetUp
    {
        public static readonly Uri node =new Uri("http://localhost:9200");
        public static ElasticClient getClient(string index) {
            ConnectionSettings settings = new ConnectionSettings(node).DefaultIndex(index).DisableDirectStreaming();
            ElasticClient client = new ElasticClient(settings);
            return client;
        }

        public static Result InsertDocument(ElasticClient client,Document document)
        {
            List<Document> list = searchIndex(client, "document");
            int maxId = list.Last().id +1;
            document.id = maxId;
            var indexResponse = client.IndexDocument(document);
            return indexResponse.Result;
        }

        public static List<Document> searchIndex(ElasticClient client,string index)
        {
            var response = client.Search<Document>(s => s
                    .Index(index)
                    .From(0)
                    .Size(1000)
                    .Query(q => q.MatchAll()));
            List<Document> list = (List<Document>)response.Documents;
            return list;

        }

        public static List<Document> searchIndexWithCriteria(ElasticClient client, string index,string criteria)
        {
            List<Document> list = null;
            if (criteria != null && criteria.Length == 0) {
                var response = client.Search<Document>(s => s
                .Index(index)
                .From(0)
                .Size(1000)
                .Query(q => q.MatchAll()));
                list = (List<Document>)response.Documents;

            }
            else {
                var response = client.Search<Document>(s => s
                        .Index(index)
                        .From(0)
                        .Size(1000)
                        .Query(q => q.QueryString(qs => qs.Query("*"+criteria+ "*"))));
                list = (List<Document>)response.Documents;
            }
            
            return list;

        }

        public static void updateDocument(ElasticClient client, Document document)
        {

            var response = client.Update<Document, Document>(document.id, d => d
          .Index("document")
          .Doc(new Document
          {
              fileName = document.fileName,
              fileContent = document.fileContent,
              dateModifier = DateTime.Now
          })) ;
        }

        public static void deleteDocument(ElasticClient client,Document document, string index)
        {
            var response = client.Delete<Document>(document.id, d => d
          .Index(index));
        }

    }
}
