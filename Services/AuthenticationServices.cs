using LunarDeckFoxyApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Threading.Tasks;

namespace LunarDeckFoxyApi.Services
{
    public class AuthenticationServices
    {
        private readonly IMongoCollection<User> _usersCollection;

        public AuthenticationServices(IOptions<LunarDeckDatabaseSettings> dbSettings)
        {
            var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<User>(dbSettings.Value.UsersCollectionName);
        }

        /// <summary>
        /// Creates a new user in the database / user sign up
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateAsync(User user) => await _usersCollection.InsertOneAsync(user);

        // TODO: get user by email / phone number and password to log in user

        public async Task<User> GetUserByEmailCredentialsAsync(User user) => await _usersCollection.Find(x =>
                x.Email == user.Email).FirstOrDefaultAsync();

        public async Task<User> GetUserByPhoneNumberCredentialsAsync(User user) =>
            await _usersCollection.Find(x =>
            x.PhoneNumber == user.PhoneNumber).FirstOrDefaultAsync();

        // TODO: update user data

        // TODO: delete user information

        // TODO: get all users from database
    }
}
