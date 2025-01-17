using ecf2.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ecf2.Services
{
    public class StatsService
    {
        private readonly IMongoCollection<EvenementStat> _EvenementStatsCollection;

        public StatsService(IOptions<EcfDatabaseSettings> ecfDatabaseSettings)
        {
            var mongoclient = new MongoClient(ecfDatabaseSettings.Value.ConnectionString);
            var mongoDatabase = mongoclient.GetDatabase(ecfDatabaseSettings.Value.DatabaseName);
            _EvenementStatsCollection = mongoDatabase.GetCollection<EvenementStat>(ecfDatabaseSettings.Value.CollectionName);
        }


        public async Task<List<EvenementStat>> GetAsync() => await _EvenementStatsCollection.Find(_ => true).ToListAsync();

        public async Task<EvenementStat?> GetAsync(int id) => await _EvenementStatsCollection.Find(x => x.EvenementId == id).FirstOrDefaultAsync();
        public async Task CreateAsync(EvenementStat Event) => await _EvenementStatsCollection.InsertOneAsync(Event);
        public async Task UpdateAsync(int id, EvenementStat updatedEvent) => await _EvenementStatsCollection.ReplaceOneAsync(x => x.EvenementId == id, updatedEvent);

    }
}
