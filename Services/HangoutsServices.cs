using LunarDeckFoxyApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LunarDeckFoxyApi.Services
{
    public class HangoutsServices
    {
        private readonly IMongoCollection<HangoutModel> _hangoutsCollection;

        public HangoutsServices(IOptions<LunarDeckDatabaseSettingsModel> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _hangoutsCollection = mongoDatabase.GetCollection<HangoutModel>(dbSettings.Value.HangoutsCollectionName);
        }

        public async Task<List<HangoutModel>> GetAsync() => await _hangoutsCollection.Find(_ => true).ToListAsync();

        public async Task<HangoutModel> GetAsync(string id) =>
            await _hangoutsCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(HangoutModel newHangout) =>
            await _hangoutsCollection.InsertOneAsync(newHangout);

        public async Task UpdateAsync(string id, HangoutModel updatedHangout) =>
            await _hangoutsCollection.ReplaceOneAsync(x => x.Id == id, updatedHangout);

        public async Task RemoveAsync(string id) =>
            await _hangoutsCollection.DeleteOneAsync(x => x.Id == id);
    }
}
