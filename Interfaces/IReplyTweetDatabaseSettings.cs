using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.Interfaces
{
    public interface IReplyTweetDatabaseSettings
    {
        public string ReplyTweetDetailsCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
