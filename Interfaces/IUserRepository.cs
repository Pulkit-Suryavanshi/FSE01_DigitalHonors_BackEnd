using com.tweetapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.Interfaces
{
    public interface IUserRepository
    {
        public void InsertUser(User user);
        public User GetUser(string userName);
        public IEnumerable<User> GetAllUsers();
        public bool ResetPassword(User user);
        public IEnumerable<User> GetByUserName(string userName);
        public bool GetUserByEmail(string email);
    }
}
