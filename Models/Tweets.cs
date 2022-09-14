using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace com.tweetapp.Models
{
    [BsonIgnoreExtraElements]
    public class Tweets
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("content")]
        public string Content { get; set; }
        [BsonElement("createdby")]
        public string CreatedBy { get; set; }
        [BsonElement("tweettime")]
        public DateTime TweetTime { get; set; }
        [BsonElement("likes")]
        public int Likes { get; set; }
    }
}
