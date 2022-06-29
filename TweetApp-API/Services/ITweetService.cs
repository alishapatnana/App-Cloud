using System.Collections.Generic;
using System.Threading.Tasks;
using TweetApp_API.Models;
using TweetApp_API.Models.Dtos.TweetDto;

namespace TweetApp_API.Services
{
    public interface ITweetService
    {
        public Task<IEnumerable<Tweets>> GetAllTweets();

        public Task<IEnumerable<Tweets>> GetUsersTweet(string userId);

        public Task<bool> Reply(string tweetId, string tweetText, string userId);

        public Task<bool> DeleteTweet(string tweetId);

        public Task<Tweets> PostTweet(string userId, CreateTweetDto tweet);

        public Task<Tweets> UpdateTweet(string tweetId, string text);

        public Task<Tweets> GetTweetByTweetId(string id);

        public Task<int> LikeOrUnlikeTweet(string tweetId, string userId);
        public Task<bool> InactivateReply(string userId);
        public Task<int> ActiveRepliesCount(string userId);


    }
}
