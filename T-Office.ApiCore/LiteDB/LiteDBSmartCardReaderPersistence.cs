using LiteDB;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using T_Office.Models;

namespace T_Office.ApiCore.LiteDB
{
    public class LiteDBSmartCardReaderPersistence : ISmartCardReaderPersistence
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly string DB_FILE_PATH;
        private string _connectionString = string.Empty;

        public LiteDBSmartCardReaderPersistence(IWebHostEnvironment hostingEnv)
        {
            this._hostingEnv = hostingEnv;
            this.DB_FILE_PATH = string.Format("{0}{1}", this._hostingEnv.ContentRootPath, @"\LiteDB\apistate.db");
            this._connectionString = string.Format(@"Filename={0}; Connection=shared", this.DB_FILE_PATH);
        }

        public void InsertSmartCardResponse(SmartCardReaderResponseModel model)
        {
            // open DB or create new if it does not exist
            using (var db = new LiteDatabase(this._connectionString))
            {
                // get or create new if it does not exist
                var collection = db.GetCollection<SmartCardReaderResponseModel>("smartcardresponses");

                model.Id = ObjectId.NewObjectId();
                collection.Insert(model);

                collection.EnsureIndex(x => x.WsConnectionId);
            }
        }

        public SmartCardReaderResponseModel GetSmartCardResponse(string wsConnectionId)
        {
            using (var db = new LiteDatabase(this._connectionString))
            {
                var collection = db.GetCollection<SmartCardReaderResponseModel>("smartcardresponses");

                return collection.Query().Where(x => x.WsConnectionId == wsConnectionId).FirstOrDefault();
            }
        }

        public bool DeleteSmartCardResponse(string wsConnectionId)
        {
            using (var db = new LiteDatabase(this._connectionString))
            {
                var collection = db.GetCollection<SmartCardReaderResponseModel>("smartcardresponses");

                var result = collection.Query().Where(x => x.WsConnectionId == wsConnectionId).FirstOrDefault();
                if (result != null)
                {
                    return collection.Delete(result.Id);
                }

                return false;
            }
        }
    }
}
