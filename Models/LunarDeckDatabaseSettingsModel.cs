namespace LunarDeckFoxyApi.Models
{
    public class LunarDeckDatabaseSettingsModel
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string ApiCollectionName { get; set; } = null!;

        public string HangoutsCollectionName { get; set; } = null!;

        public string UsersCollectionName { get; set; } = null!;
    }
}
