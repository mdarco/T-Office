using LiteDB;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using T_Office.Models;

namespace T_Office.ApiCore.LiteDB
{
    public class LiteDBAgentDataPersistence : IAgentDataPersistence
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly string DB_FILE_PATH;

        public LiteDBAgentDataPersistence(IWebHostEnvironment hostingEnv)
        {
            this._hostingEnv = hostingEnv;
            this.DB_FILE_PATH = string.Format("{0}{1}", this._hostingEnv.ContentRootPath, @"\LiteDB\apistate.db");
        }

        public void InsertAgentData(AgentDataModel model)
        {
            // open DB or create new if it does not exist
            using (var db = new LiteDatabase(this.DB_FILE_PATH))
            {
                // get or create new if it does not exist
                var collection = db.GetCollection<AgentDataModel>("agents");

                // since AgentId is unique, first delete one if it exists
                var existing = collection.Query().Where(x => x.AgentId == model.AgentId).FirstOrDefault();
                if (existing != null)
                {
                    collection.Delete(existing.Id);
                }

                model.Id = ObjectId.NewObjectId();
                collection.Insert(model);

                collection.EnsureIndex(x => x.AgentId);
            }
        }

        public AgentDataModel GetAgentData(string agentId)
        {
            using (var db = new LiteDatabase(this.DB_FILE_PATH))
            {
                var collection = db.GetCollection<AgentDataModel>("agents");

                return collection.Query().Where(x => x.AgentId == agentId).FirstOrDefault();
            }
        }

        public bool DeleteAgentData(string agentId)
        {
            using (var db = new LiteDatabase(this.DB_FILE_PATH))
            {
                var collection = db.GetCollection<AgentDataModel>("agents");

                var result = collection.Query().Where(x => x.AgentId == agentId).FirstOrDefault();
                if (result != null)
                {
                    return collection.Delete(result.Id);
                }

                return false;
            }
        }
    }
}
