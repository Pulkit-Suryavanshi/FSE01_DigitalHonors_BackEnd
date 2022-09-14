using com.tweetapp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.DatabaseSettings
{
    public class TweetDatabaseSettings : ITweetDatabaseSettings
    {
        public string TweetDetailsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
