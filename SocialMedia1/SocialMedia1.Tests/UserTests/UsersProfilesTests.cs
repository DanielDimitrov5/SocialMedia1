using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SocialMedia1.Data;
using SocialMedia1.Services.Posts;
using SocialMedia1.Services.Users;
using SocialMedia1.Tests.Data;
using System.Linq;

namespace SocialMedia1.Tests.Users
{
    [TestFixture]
    public class UsersProfilesTests
    {
        public static DbContextOptions<ApplicationDbContext> dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("SocialMedia1Tests")
            .Options;


        private ApplicationDbContext context;
        private IUserProfileService userProfileService;

        [OneTimeSetUp]
        public void SetUp()
        {
            context = new ApplicationDbContext(dbOptions);
            context.Database.EnsureCreated();

            DataSeeder.Seed(context);

            userProfileService = new UserProfileService(context, new PostService(context));
        }

        [Test]
        public void AddUserProfileAsyncCreatesUserProfile()
        {
            string newUserId = DataSeeder.User().Id;

            userProfileService.AddUserProfileAsync(newUserId);

            Assert.True(context.UserProfiles.Any(x => x.Id == newUserId));
        }

        [Test]
        public void EditUserProfileAsyncWorksCorrectly()
        {
            var user = DataSeeder.UserProfiles()[0];

            userProfileService.EditUserProfileAsync(user.Id, "changed nickname", user.Name, user.Surname, user.IsPrivate, "changed bio", user.ImageUrl);

            Assert.AreNotEqual(user.Nickname, context.UserProfiles.Find(user.Id).Nickname);
            Assert.AreNotEqual(user.Bio, context.UserProfiles.Find(user.Id).Bio);

            userProfileService.EditUserProfileAsync(user.Id, user.Nickname, user.Name, user.Surname, user.IsPrivate, user.Bio, user.ImageUrl);

            Assert.AreEqual(user.Nickname, context.UserProfiles.Find(user.Id).Nickname);
        }

        [Test]
        public void AddUserProfileAsyncSetsCorrectNickname()
        {
            var user = DataSeeder.User();

            string newUserId = user.Id;

            userProfileService.AddUserProfileAsync(newUserId);

            string expected = user.Email.Split('@')[0];

            string actual = context.UserProfiles.Find(newUserId).Nickname;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetUserProfileDataAsyncReturnsNullIfUserNotFound()
        {
            var actual = userProfileService.GetUserProfileDataAsync("unexisting guid");

            Assert.Null(actual.Result);
        }

        [Test]
        public void GetUserProfileDataAsyncReturnsCorrectUser()
        {
            int userIndex = 0;

            var actual = userProfileService.GetUserProfileDataAsync(DataSeeder.UserProfiles()[userIndex].Id);

            Assert.AreEqual(DataSeeder.UserProfiles()[userIndex].Nickname, actual.Result.Nickname);
        }

        [Test]
        [TestCase("fee859e9-7b26-4cf8-a3c5-7cea60c13101")] //User with 2 follow requests
        [TestCase("99c88001-1221-4ff7-a0e9-f5efac98fe9e")] //User with 0 follow requests
        public void GetAllFollowRequestsReturnsCorrectData(string userId)
        {
            var expectedRequestCount = DataSeeder.FollowRequests().Where(x => x.UserId == userId).Count();

            var actual = userProfileService.GetAllFollowRequests(userId);

            Assert.AreEqual(expectedRequestCount, actual.Result.Count);
        }

        [Test]
        [TestCase("fee859e9-7b26-4cf8-a3c5-7cea60c13101")] //User with 1 follower
        [TestCase("71b0265a-1994-4b4a-a824-8a4401aae60e")] //User with 0 followers
        public void GetAllFollowersReturnsCorrectData(string userId)
        {
            var expected = context.UserProfiles.Find(userId).FollowedBy.Count;

            var actual = userProfileService.GetAllFollowers(userId);

            Assert.AreEqual(expected, actual.Profiles.Count);
        }

        [Test]
        [TestCase("fee859e9-7b26-4cf8-a3c5-7cea60c13101", "99c88001-1221-4ff7-a0e9-f5efac98fe9e")]
        public void GetAllFollowersConcatenatesFullName(string userId, string followerId)
        {
            var user = DataSeeder.UserProfiles()[0];

            var expected = user.Name + " " + user.Surname;

            var actual = userProfileService.GetAllFollowers(followerId).Profiles.First(x => x.Id == userId).Name;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase("fee859e9-7b26-4cf8-a3c5-7cea60c13101")] //User with 1 following
        [TestCase("71b0265a-1994-4b4a-a824-8a4401aae60e")] //User with 0 following
        public void GetAllFollowingAsyncReturnsCorrectData(string userId)
        {
            var expected = context.UserProfiles.Find(userId).Follows.Count;

            var actual = userProfileService.GetAllFollowingAsync(userId);

            Assert.AreEqual(expected, actual.Result.Profiles.Count);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }
    }
}
