using com.tweetapp.Interfaces;
using com.tweetapp.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> users;

        public UserRepository(IUserDatabaseSettings userDatabaseSettings)
        {
            var mongoClient = new MongoClient(userDatabaseSettings.ConnectionString);
            var database = mongoClient.GetDatabase(userDatabaseSettings.DatabaseName);
            users = database.GetCollection<User>(userDatabaseSettings.UserDetailsCollectionName);
        }
        public IEnumerable<User> GetAllUsers()
        {
            try
            {
                return users.Find<User>(_ => true).ToList();
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<User> GetByUserName(string userName)
        {
            try
            {
                return users.Find<User>(u => u.UserName.Contains(userName)).ToList();
            }
            catch
            {
                return null;
            }
        }

        public User GetUser(string userName)
        {
            try
            {
                User user = users.Find<User>(u => u.UserName == userName).FirstOrDefault();
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public bool GetUserByEmail(string email)
        {
            User user = users.Find<User>(u => u.Email == email).FirstOrDefault();
            if (user != null)
            {
                return false;
            }
            return true;
        }

        public void InsertUser(User user)
        {
            try
            {
                users.InsertOne(user);
            }
            catch
            {
                throw new Exception("User Creation Failed");
            }
        }

        public bool ResetPassword(User user)
        {
            var filter = Builders<User>.Filter.Eq(t => t.UserName, user.UserName);
            var update = Builders<User>.Update.Set(t => t.Password, new PasswordHasher<User>().HashPassword(user, user.Password));
            var options = new UpdateOptions { IsUpsert = false };
            UpdateResult updateResult = users.UpdateOne(filter, update, options);
            if (updateResult.MatchedCount > 0)
            {
                return true;
            }
            return false;
        }
    }
}
