namespace RoyPhishingProj.DBLayer
{
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using BusinessLogicLayer.dto;
    using System.Threading.Tasks;
    using RoyPhishingProj.BusinessLogicLayer.Enums;

    public class MongoDBService
    {
        private readonly IMongoCollection<EmailDto> _EmailDtosCollection;
        private readonly IMongoCollection<EmailMappingDto> _EmailMappingDtoDtosCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _EmailDtosCollection = mongoDatabase.GetCollection<EmailDto>(mongoDBSettings.Value.EmailDtosCollectionName);
            _EmailMappingDtoDtosCollection = mongoDatabase.GetCollection<EmailMappingDto>(mongoDBSettings.Value.EmailMappingDtosCollectionName);
        }

        public async Task SetEmailDto(EmailDto emailDto)
        {
            await _EmailDtosCollection.InsertOneAsync(emailDto);
        }

        public async Task UpdateEmailDto(string email, string status)
        {
            var filter = Builders<EmailDto>.Filter.Eq(pa => pa.Email, email);

            var update = Builders<EmailDto>.Update.Set(pa => pa.Status, status);

            var result = await _EmailDtosCollection.UpdateOneAsync(filter, update);

            if (result.ModifiedCount == 0)
            {
                throw new Exception("Update failed!");
            }
        }

        public async Task SetEmailMapping(EmailMappingDto emailMappingDto)
        {
            await _EmailMappingDtoDtosCollection.InsertOneAsync(emailMappingDto);
        }

        public async Task<List<EmailMappingDto>> GetAllEmailMapping()
        {
            var allDocuments = await _EmailMappingDtoDtosCollection.Find(Builders<EmailMappingDto>.Filter.Empty).ToListAsync();

            return allDocuments;
        }

        public async Task<List<EmailDto>> GetAllPhishingAttempts()
        {
            var allDocuments = await _EmailDtosCollection.Find(Builders<EmailDto>.Filter.Empty).ToListAsync();

            return allDocuments;
        }
    }
}
