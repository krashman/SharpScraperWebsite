using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;


namespace SharpScraperWebsite.Controllers
{

    // Per prenderei risultati da un database noSQL
    public class Database
    {
        MongoClient client;
        IMongoDatabase database;
        IMongoCollection<BsonDocument> collection;

        // Inizializzo il database
        public Database()
        {
            this.client = new MongoClient("mongodb://localhost:27017");
            this.database = client.GetDatabase("SharpScraper");
            this.collection = database.GetCollection<BsonDocument>("findings_");
        }

        // Per trovare gli ultimi n paste
        public List<BsonDocument> GetLastNEntries(int limit)
        {
            var sort = Builders<BsonDocument>.Sort.Descending("date");

            var documents = collection.Find(new BsonDocument())
                                                  .Sort(sort) 
                                                  .Limit(limit)
                                                  .ToList();
            return documents;
        }
    }




    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Archive()
        {
            var mongoDB = new Database();
            var messaggio = mongoDB.GetLastNEntries(10);
            List<string> lista = new List<string>();
            string risposta = "";

            // Creo una lsita id risultati
            for (int i = 0; i < messaggio.Count(); i++)
            {
                // Non mi fido molto di questo casting, c'è un metodo migliore e più sicuro?
                risposta = (string) messaggio[i]["text"];
                lista.Add(risposta);
            }

            ViewData["lista"] = lista;

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = " ";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
