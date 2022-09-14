using com.tweetapp.Interfaces;
using com.tweetapp.Models;
using com.tweetapp.Repositories;
using com.tweetapp.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace com.tweetapp.Controllers
{
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class tweetsController : ControllerBase
    {
        private UserRepository users;
        private TweeterRepository tweets;

        public tweetsController(UserRepository userRepository, TweeterRepository tweeterRepository)
        {
            users = userRepository;
            tweets = tweeterRepository;
        }


        //GET: api/v1.0/<tweetsController>/users/all
        [HttpGet]
        [Route("users/all")]
        public ObjectResult GetAllUser()
        {
            return Ok(users.GetAllUsers());
        }

        //GET: api/v1.0/<tweetsController>/user/search/username
        [HttpGet]
        [Route("user/search/{username}")]
        public ObjectResult GetUserByUsername(string username)
        {
            return Ok(users.GetByUserName(username));
        }

        //POST: api/v1.0/<tweetsController>/register
        [HttpPost("register")]
        public ObjectResult RegisterNewUser([FromBody] User user)
        {
            if (!user.ValidateRequired())
                return StatusCode(400, new { msg = "All are required fields" });
            if (!UserValidation.UserName(user.UserName))
                return StatusCode(400, new { msg = "Username must be within 8-20 alphanumeric characters" });
            if (!UserValidation.Password(user.Password))
                return StatusCode(400, new { msg = UserValidation.PasswordErrorMessage() });
            if (!UserValidation.PhoneNo(user.ContactNo))
                return StatusCode(400, new { msg = "Invalid Phone Number" });
            if (!UserValidation.Email(user.Email))
                return StatusCode(400, new { msg = "Invalid Email Id" });
            var ExistingUser = users.GetUser(user.UserName);
            if (ExistingUser != null)
                return StatusCode(400, new { msg = "UserName Exist" });
            if (!users.GetUserByEmail(user.Email))
                return StatusCode(400, new { msg = "Email Id Exist" });
            user.Password = new PasswordHasher<User>().HashPassword(user, user.Password);
            users.InsertUser(user);
            return StatusCode(201, new { msg = "User Created Successfully!" });
        }

        //POST: api/v1.0/<tweetsController>/login
        [HttpPost("login")]
        public ObjectResult UserLogin([FromBody] User user)
        {
            User authUser = users.GetUser(user.UserName);
            if (authUser != null)
            {
                if (new PasswordHasher<User>().VerifyHashedPassword(user, authUser.Password, user.Password) == PasswordVerificationResult.Success)
                    return Ok(new { loggedIn = true, msg = "Authentication Successful." });
                return StatusCode(401, new { loggedIn = false, msg = "Password Incorrect." });
            }
            return StatusCode(401, new { loggedIn = false, msg = $"User with username:{user.UserName} doesn't exist." });
        }

        //GET: api/v1.0/<tweetsController>/<username>/forgot
        [HttpPost]
        [Route("{username}/forgot")]
        public ObjectResult ForgotPassword([FromBody] User user)
        {
            try
            {
                if (!UserValidation.Password(user.Password))
                    return StatusCode(400, new { msg = UserValidation.PasswordErrorMessage() });
                bool reset = users.ResetPassword(user);
                if (reset)
                    return Ok(new { msg = "Password Reset Successful." });
                return StatusCode(404, new { msg = "Couldn't reset password." });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { msg = "Something went wrong.", error = e });
            }
        }

        //GET: api/v1.0/<tweetsController>/all
        [HttpGet("all")]
        public ObjectResult GetAllTweet()
        {
            return Ok(tweets.GetAllTweets());
        }

        //POST: api/v1.0/<tweetsController>/<username>/add
        [HttpPost]
        [Route("{username}/add")]
        public ObjectResult PostNewTweet([FromBody] Tweets post)
        {
            try
            {
                tweets.InsertTweet(post);
                return StatusCode(201, new { msg = "Tweet created successfully.", id = post.Id });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { msg = "Something went wrong", error = e });
            }
        }

        //PUT: api/v1.0/<tweetsController>/<username>/update/id
        [HttpPut]
        [Route("{username}/update/{id}")]
        public ObjectResult UpdateTweet(string id, [FromBody] Tweets post)
        {
            try
            {
                post.Id = id;
                tweets.UpdateTweet(post);
                return Ok(new { msg = "Tweet Updated Successfully" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { msg = "Something went wrong", error = e });
            }
        }

        //DELETE: api/v1.0/<tweetsController>/<username>/delete/id
        [HttpDelete]
        [Route("{username}/delete/{id}")]
        public ObjectResult DeleteTweet(string id)
        {
            try
            {
                tweets.DeleteTweet(id);
                return Ok(new { msg = "Tweet deleted successfully" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { msg = "Something went wrong", error = e });
            }
        }

        //PUT: api/v1.0/<tweetsController>/<username>/like/id
        [HttpPut]
        [Route("{username}/like/{id}")]
        public ObjectResult Like(string id, [FromBody] Tweets post)
        {
            try
            {
                post = tweets.GetTweetById(id);
                tweets.LikeTweet(post);
                return Ok(new { msg = "Tweet Liked successfully" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { msg = "Something went wrong", error = e });
            }
        }

        //POST: api/v1.0/<tweetsController>/<username>/reply/id
        [HttpPost]
        [Route("{username}/reply/{id}")]
        public ObjectResult ReplyTweet([FromBody] Reply reply)
        {
            try
            {
                tweets.ReplyToTweet(reply);
                return StatusCode(201, new { msg = "Reply Added Successfully" });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { msg = "Something went wrong", error = e });
            }
        }

        //GET: api/v1.0/<tweetsController>/<username>
        [HttpGet("{userName}")]
        public ObjectResult GetUserNameByTweets(string userName)
        {
            return Ok(tweets.GetTweetsByUserName(userName));
        }

        //GET: api/v1.0/<tweetsController>/reply/{tweetId}
        [HttpGet]
        [Route("reply/{tweetId}")]
        public ObjectResult GetReplyToTweet(string tweetId)
        {
            return Ok(tweets.GetReplyList(tweetId));
        }
        //GET: api/v1.0/<tweetsController>/details/{tweetId}
        [HttpGet("details/{tweetId}")]
        public ObjectResult GetById(string tweetId)
        {
            return Ok(tweets.GetTweetById(tweetId));
        }
    }
}
