using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LunarDeckFoxyApi.Models
{
    public class HangoutModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string LunarId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public string Duration { get; set; }

        public string Location { get; set; }

        public string CreatedBy { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime LastUpdatedAt { get; set;} = DateTime.UtcNow;

        //public List<Rsvp> RsvpList { get; set; }

    }
}
