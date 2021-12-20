using LunarDeckFoxyApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LunarDeckFoxyApi.Services
{
    public class HangoutsServices
    {
        private readonly IMongoCollection<Hangout> _hangoutsCollection;

        public HangoutsServices(IOptions<LunarDeckDatabaseSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _hangoutsCollection = mongoDatabase.GetCollection<Hangout>(dbSettings.Value.HangoutsCollectionName);
        }

        public async Task<List<Hangout>> GetAsync() => await _hangoutsCollection.Find(_ => true).ToListAsync();

        public async Task<Hangout> GetAsync(string id) =>
            await _hangoutsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Hangout newHangout) =>
            await _hangoutsCollection.InsertOneAsync(newHangout);

        public async Task UpdateAsync(string id, Hangout updatedHangout) =>
            await _hangoutsCollection.ReplaceOneAsync(x => x.Id == id, updatedHangout);

        public async Task RemoveAsync(string id) =>
            await _hangoutsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
