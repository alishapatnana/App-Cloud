using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetApp_API.Data.IRepository;
using TweetApp_API.Models;

namespace TweetApp_API.Data
{
    public class TweetRepository : ITweetRepository
    {
        private readonly IMongoCollection<Tweets> _tweets;
        public TweetRepository(IMongoClient mongoClient, IConfiguration config)
        {
            var database = mongoClient.GetDatabase("tweetapp");
            _tweets = database.GetCollection<Tweets>(config.GetValue<string>("TweetAppDBSettings:TweetsCollectionName"));
        }

        public async Task<bool> DeleteTweet(string tweetId)
        {
            var result = await _tweets.DeleteOneAsync(s => s.Id == tweetId);
            return result.DeletedCount > 0;
        }

        public async Task<IEnumerable<Tweets>> GetAllTweets()
        {
            return await _tweets.Find(s => true).ToListAsync();
        }

        public async Task<IEnumerable<Tweets>> GetUsersTweet(string userId)
        {
            return await _tweets.Find(s => s.UserId == userId).ToListAsync();
        }

        public async Task<bool> Reply(string tweetId, ReplyTweets reply)
        {
            reply.Notification = true;
            var pushElement = Builders<Tweets>.Update.Push(t => t.Replies, reply);
            var result = await _tweets.UpdateOneAsync(s => s.Id == tweetId, pushElement);
            return result.ModifiedCount > 0;
        }

        public async Task<Tweets> PostTweet(Tweets tweet)
        {
            await _tweets.InsertOneAsync(tweet);
            return tweet;
        }

        public async Task<Tweets> UpdateTweet(string tweetId, string text)
        {
            var result = await _tweets.FindOneAndUpdateAsync(s => s.Id == tweetId, Builders<Tweets>.Update.Set(u => u.TweetText, text));
            return result;
        }

        public async Task<Tweets> GetTweetByTweetId(string tweetId)
        {
            var result = await _tweets.Find(m => m.Id == tweetId).FirstOrDefaultAsync();
            return result;
        }

        public async Task<int> LikeOrUnlikeTweet(string tweetId, string userId)
        {
            var tweetDetail = await _tweets.Find(m => m.Id == tweetId).FirstOrDefaultAsync();
            int likes = tweetDetail.Likes;
            bool isAlreadyLiked = tweetDetail.LikedBy.Contains(userId); ;
            if (isAlreadyLiked)
            {
                likes = likes - 1;
                var pushElement = Builders<Tweets>.Update.Combine(
                   Builders<Tweets>.Update.Pull(x => x.LikedBy, userId),
                   Builders<Tweets>.Update.Set(x => x.Likes, likes)
                   );
                await _tweets.UpdateOneAsync(s => s.Id == tweetId, pushElement);
            }
            else
            {
                likes = likes + 1;
                var pushElement = Builders<Tweets>.Update.Combine(
                    Builders<Tweets>.Update.Push(x => x.LikedBy, userId),
                    Builders<Tweets>.Update.Set(x => x.Likes, likes)
                    );
                pushElement.Set(t => t.Likes, likes);
                await _tweets.UpdateOneAsync(s => s.Id == tweetId, pushElement);
            }
            return likes;
        }
        public async Task<bool> InactivateReply(string userId)
        {

            var tweets = await _tweets.Find(tweet => tweet.UserId == userId).ToListAsync();
            foreach (Tweets tweet in tweets)
            {
                foreach (ReplyTweets replyTweets in tweet.Replies)
                {
                    replyTweets.Notification = false;
                }
                var pushElement = Builders<Tweets>.Update.Set(x => x.Replies, tweet.Replies);
                await _tweets.UpdateOneAsync(s => s.Id == tweet.Id, pushElement);

            }
            return true;
        }

        public async Task<int> ActiveRepliesCount(string userId)
        {
            int count = 0;
            var tweets = await _tweets.Find(tweet => tweet.UserId == userId).ToListAsync();
            foreach (Tweets tweet in tweets)
            {
                foreach (ReplyTweets replyTweets in tweet.Replies)
                {
                    if (replyTweets.Notification == true)
                        count++;
                }
            }
            return count;
        }
    }
}
