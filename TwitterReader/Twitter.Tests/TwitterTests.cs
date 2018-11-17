using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TwitterAccess;

namespace Twitter.Tests
{
	[TestClass]
    public class TwitterTests
    {
		private TwitterHttpClient _client;

		[TestInitialize]
		public void Setup()
		{
			TwitterCredentials twitterCreds = null;
			string jsonFilePath = Path.Combine(
				@"e:\FileSync\SyncProjects\TwitterReader\TwitterReader\",
				@"TwitterCredentials.json");
			Assert.IsTrue(File.Exists(jsonFilePath));
			twitterCreds = JsonHelper.DeserializeFromFile<TwitterCredentials>(jsonFilePath);
			Assert.IsNotNull(twitterCreds);
			_client = new TwitterHttpClient(twitterCreds);
			Assert.IsNotNull(_client);
		}

		[TestCleanup]
		public void Teardown()
		{
		}

		[TestMethod]
		public void TestAuthentication()
		{
			var loginUser = _client.GetAuthenticatedUser();
			Assert.IsNotNull(loginUser);
			System.Console.WriteLine($"Login User : {loginUser}");
		}

		[TestMethod]
		public void GettingMyFriends_ReturnsAtLeast10()
		{
			var result = _client.GetFriends(13075422);
			Assert.IsNotNull(result);
			Assert.IsTrue(result.Count > 9);
			foreach (var item in result)
			{
				System.Console.WriteLine(item.ScreenName);
			}
		}

		[TestMethod]
		public void GettingThijsFriends_IsNotAllowed()
		{
			var result = _client.GetFriends(2436022880);  // 403 on friends of friends
			Assert.IsNotNull(result);  //  but empty list 403 on the ID query
			Assert.IsTrue(result.Count == 0);
		}

		[TestMethod]
		public void GettingThijsTweets()
		{
			List<TweetEntity> tweetList =
				_client.GetUserTweetList(
					2436022880,
					20,
					includeRetweet: true);
			Assert.IsTrue(tweetList.Count == 20);
		}

		[TestMethod]
		public void GettingOneThijsTweet()
		{
			List<TweetEntity> tweetList =
				_client.GetUserTweetList(
					2436022880,
					1,
					includeRetweet: true);
			Assert.IsTrue(tweetList.Count == 1);
			var tweet = tweetList.FirstOrDefault();
			DumpToConsole(tweet);
		}

		private static void DumpToConsole(TweetEntity tweet)
		{
			System.Console.WriteLine($"FullText: {tweet.FullText}");
			System.Console.WriteLine($"At      : {tweet.CreatedAt}");
			System.Console.WriteLine($"By      : {tweet.CreatedBy}");
			if (tweet.LegacyEntities != null)
			{
				if (tweet.LegacyEntities.UrlList != null)
				{
					System.Console.WriteLine($"Urls    : {tweet.LegacyEntities.UrlList.Count()}");
					if (tweet.LegacyEntities.UrlList.Any())
					{
						foreach (var item in tweet.LegacyEntities.UrlList)
						{
							System.Console.WriteLine($"  {item.Url}");
						}
					}
				}
				if (tweet.LegacyEntities.MediaList != null)
					System.Console.WriteLine($"Media   : {tweet.LegacyEntities.MediaList.Count()}");
			}
		}
	}
}
