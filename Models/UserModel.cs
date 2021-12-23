using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace LunarDeckFoxyApi.Models
{

    public class UserModel
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } 

        public string LunarId { get; set; }

        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [BsonIgnore]
        public string Password { get; set; }

        [BsonIgnore]
        public string ConfirmPassword { get; set; }

        public string PasswordHash { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime LastUpdatedAt { get; set; }
        
        [BsonIgnore]
        public string JwtToken { get; set; }
    }
}
