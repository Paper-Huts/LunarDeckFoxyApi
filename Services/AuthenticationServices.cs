using LunarDeckFoxyApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace LunarDeckFoxyApi.Services
{
    public class AuthenticationServices
    {
        private readonly IMongoCollection<UserModel> _usersCollection;

        public AuthenticationServices(IOptions<LunarDeckDatabaseSettingsModel> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<UserModel>(dbSettings.Value.UsersCollectionName);
        }

        /// <summary>
        /// Creates a new user in the database / user sign up
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateAsync(UserModel user) => await _usersCollection.InsertOneAsync(user);

        // TODO: get user by email / phone number and password to log in user

        public async Task<UserModel> GetUserByEmailCredentialsAsync(UserModel user) => 
            await _usersCollection.Find(x => 
            x.Email == user.Email && 
            x.PasswordHash == user.PasswordHash).FirstOrDefaultAsync();

        public async Task<UserModel> GetUserByPhoneNumberCredentialsAsync(UserModel user) =>
            await _usersCollection.Find(x =>
            x.PhoneNumber == user.PhoneNumber && 
            x.PasswordHash == user.PasswordHash).FirstOrDefaultAsync();

        // TODO: update user data

        // TODO: delete user information

        // TODO: get all users from database
    }
}
