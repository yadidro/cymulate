namespace RoyPhishingProj.DBLayer
{
    public class MongoDBSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string EmailMappingDtosCollectionName { get; set; } = null!;
        public string EmailDtosCollectionName { get; set; } = null!;
    }
}
