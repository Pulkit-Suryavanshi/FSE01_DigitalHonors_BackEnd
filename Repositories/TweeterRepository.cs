using com.tweetapp.Interfaces;
using com.tweetapp.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.Repositories
{
    public class TweeterRepository : ITweetService, IReplyTweetService
    {
        private readonly IMongoCollection<Tweets> tweets;
        private readonly IMongoCollection<Reply> replies;

        public TweeterRepository(ITweetDatabaseSettings tweetDatabaseSettings,IReplyTweetDatabaseSettings replyTweetDatabaseSettings)
        {
            var client = new MongoClient(tweetDatabaseSettings.ConnectionString);
            var database = client.GetDatabase(tweetDatabaseSettings.DatabaseName);
            tweets = database.GetCollection<Tweets>(tweetDatabaseSettings.TweetDetailsCollectionName);

            client = new MongoClient(replyTweetDatabaseSettings.ConnectionString);
            database = client.GetDatabase(replyTweetDatabaseSettings.DatabaseName);
            replies = database.GetCollection<Reply>(replyTweetDatabaseSettings.ReplyTweetDetailsCollectionName);
        }
        public void DeleteTweet(string id)
        {
            tweets.DeleteOne(t => t.Id == id);
        }

        public IEnumerable<Tweets> GetAllTweets()
        {
            try
            {
                return tweets.Find<Tweets>(_ => true).ToList();
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Reply> GetReplyList(string tweetId)
        {
            return replies.Find<Reply>(reply => reply.TweetId == tweetId).ToList();
        }

        public Tweets GetTweetById(string id)
        {
            return tweets.Find<Tweets>(t => t.Id == id).FirstOrDefault();
        }

        public IEnumerable<Tweets> GetTweetsByUserName(string userName)
        {
            try
            {
                return tweets.Find<Tweets>(u => u.CreatedBy == userName).ToList();
            }
            catch
            {
                return null;
            }
        }

        public void InsertTweet(Tweets tweet)
        {
            try
            {
                tweet.TweetTime = DateTime.Now;
                tweets.InsertOne(tweet);
            }
            catch
            {
                throw new Exception("Couldn't create Tweet.");
            }
        }

        public void LikeTweet(Tweets tweet)
        {
            var filter = Builders<Tweets>.Filter.Eq(t => t.Id, tweet.Id);
            var update = Builders<Tweets>.Update.Set(t => t.Likes, tweet.Likes + 1);
            var options = new UpdateOptions { IsUpsert = true };
            tweets.UpdateOne(filter, update, options);
        }

        public void ReplyToTweet(Reply reply)
        {
            try
            {
                reply.ReplyTime = DateTime.Now;
                replies.InsertOne(reply);
            }
            catch
            {
                throw new Exception("Couldn't reply to tweet.");
            }
        }

        public void UpdateTweet(Tweets tweet)
        {
            tweet.TweetTime = DateTime.Now;
            var filter = Builders<Tweets>.Filter.Eq(t => t.Id, tweet.Id);
            var update = Builders<Tweets>.Update.Set(t => t.Content, tweet.Content);
            var options = new UpdateOptions { IsUpsert = true };
            tweets.UpdateOne(filter, update, options);
        }
    }
}
