using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace com.tweetapp.Models
{
    [BsonIgnoreExtraElements]
    public class Reply
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("content")]
        public string Content { get; set; }
        [BsonElement("repliedby")]
        public string RepliedBy { get; set; }
        [BsonElement("replytime")]
        public DateTime ReplyTime { get; set; }
        [BsonElement("tweetId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TweetId { get; set; }
    }
}
