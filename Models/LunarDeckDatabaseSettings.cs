namespace LunarDeckFoxyApi.Models
{
    public class LunarDeckDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ApiCollectionName { get; set; } = null!;

        public string HangoutsCollectionName { get; set; } = null!;
    }
}
