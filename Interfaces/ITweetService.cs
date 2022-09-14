using com.tweetapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.Interfaces
{
    public interface ITweetService
    {
        public void InsertTweet(Tweets tweets);
        public IEnumerable<Tweets> GetTweetsByUserName(string userName);
        public IEnumerable<Tweets> GetAllTweets();
        public Tweets GetTweetById(string id);
        public void UpdateTweet(Tweets tweets);
        public void LikeTweet(Tweets tweets);
        public void ReplyToTweet(Reply reply);
        public void DeleteTweet(string id);
    }
}
