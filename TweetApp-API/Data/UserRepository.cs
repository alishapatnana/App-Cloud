using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using TweetApp_API.Data.IRepository;
using TweetApp_API.Models;
using TweetApp_API.Models.Dtos.UserDto;

namespace TweetApp_API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<Users> _users;
        private readonly IMongoCollection<Tweets> _tweets;
        public UserRepository(IMongoClient mongoClient, IConfiguration config)
        {
            var database = mongoClient.GetDatabase("tweetapp");
            _users = database.GetCollection<Users>(config.GetValue<string>("TweetAppDBSettings:UsersCollectionName"));
            _tweets = database.GetCollection<Tweets>(config.GetValue<string>("TweetAppDBSettings:TweetsCollectionName"));
        }
        public async Task<bool> CreateUserAsync(Users userDetails)
        {
            await _users.InsertOneAsync(userDetails);
            return true;
        }

        public async Task<IEnumerable<Users>> GetAllUsersAsync()
        {
            return await _users.Find(s => true).ToListAsync();
        }

        public async Task<Users> GetUserAsync(string userId)
        {
            return await _users.Find(s => s.EmailId == userId).FirstOrDefaultAsync();
        }

        public async Task<Users> LoginUser(UserCredentials credential)
        {
            return await _users.Find(s => s.EmailId == credential.EmailId && s.Password == credential.Password).FirstOrDefaultAsync();
        }

        public async Task<bool> IsUserAlreadyExist(string userId)
        {
            return await _users.Find(s => s.EmailId == userId).FirstOrDefaultAsync() != null;
        }

        public async Task<bool> updatePassword(string userId, string newPassword)
        {
            var result = await _users.UpdateOneAsync(t => t.EmailId == userId, Builders<Users>.Update.Set(m => m.Password, newPassword));
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> CheckSecurityCredential(ForgotPasswordDto credential)
        {
            var result = await _users.Find(m => m.EmailId == credential.EmailId &&
            m.SecurityQuestion == credential.SecurityQuestion &&
            m.SecurityAnswer.ToLower() == credential.SecurityAnswer.ToLower()).FirstOrDefaultAsync();

            return result != null;
        }

        public async Task<bool> UpdateUser(string userId, Users userDetails)
        {
            var result = await _users.Find(s => s.EmailId == userId).FirstOrDefaultAsync();
            result.FirstName = userDetails.FirstName;
            result.LastName = userDetails.LastName;
            result.DateOfBirth = userDetails.DateOfBirth;
            result.Gender = userDetails.Gender;
            var updatedResult = await _users.ReplaceOneAsync(x => x.EmailId == userId, result);
            var tweets = await _tweets.Find(tweet => tweet.UserId == userId).ToListAsync();
            foreach (Tweets tweet in tweets)
            {
                tweet.FirstName = userDetails.FirstName;
                tweet.LastName = userDetails.LastName;
                await _tweets.ReplaceOneAsync(s => s.Id == tweet.Id, tweet);
            }
            result = await _users.Find(s => s.EmailId == userId).FirstOrDefaultAsync();
            return true;
        }

        public async Task<IEnumerable<Users>> GetUsersByUserIdAsync(string id)
        {
            return await _users.Find(
               new BsonDocument { { "emailId", new BsonDocument { { "$regex", id }, { "$options", "i" } } } }).ToListAsync();

        }
        public async Task<string> SendMail(string id)
        {
            string otpPassword = CreateRandomPassword();
            try
            {
                MailMessage Msg = new MailMessage();
                // Sender e-mail address.
                Msg.From = new MailAddress("tweetapphelpdesk@gmail.com");
                // Recipient e-mail address.
                Msg.To.Add(id);
                Msg.Subject = "Your Sign in Code";
                Msg.Body = "Hi " + id + "," + Environment.NewLine + Environment.NewLine + "We have received your request for sign in code to use with your account." + Environment.NewLine + Environment.NewLine +
                    "Your sign in code is " + otpPassword + Environment.NewLine + Environment.NewLine + "If you didn't request this code, You can safely ignore this email. Some one else might have typed your email address my mistake" +
                    Environment.NewLine + Environment.NewLine +
                   "Thanks," + Environment.NewLine + "The TweetApp Support Team";
                // your remote SMTP server IP.
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = "tweetapphelpdesk@gmail.com",
                    Password = "bpjqdohxbomgmnmc"
                };
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(Msg);
                return otpPassword;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
                return null;
            }
        }
        public string CreateRandomPassword()
        {
            //string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            string _allowedChars = "0123456789";
            Random randNum = new Random();
            char[] chars = new char[6];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < 6; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

    }
}
