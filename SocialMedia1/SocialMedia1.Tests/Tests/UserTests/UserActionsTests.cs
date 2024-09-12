using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SocialMedia1.Data;
using SocialMedia1.Services.Users;
using SocialMedia1.Tests.Data;

namespace SocialMedia1.Tests.Tests.UsersTests
{
    [TestFixture]
    public class UserActionsTests
    {
        private static DbContextOptions<ApplicationDbContext> dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("SocialMedia1Tests")
           .Options;


        private ApplicationDbContext context;
        private IUserActionsService userActionsService;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            context = new ApplicationDbContext(dbOptions);
            await context.Database.EnsureCreatedAsync();

            await DataSeeder.SeedAsync(context);

            userActionsService = new UserActionsService(context);
        }

        [Test]
        public void FollowUserAsyncHandlesInvalidInput()
        {
            var user = context.UserProfiles.Find(DataSeeder.UserProfiles()[1].Id);

            Assert.DoesNotThrowAsync(async () => await userActionsService.FollowUserAsync(user.Id, "fake-user"));
            Assert.DoesNotThrowAsync(async () => await userActionsService.FollowUserAsync("fake_user", user.Id));
            Assert.DoesNotThrowAsync(async () => await userActionsService.FollowUserAsync("fake_user", "fake-user"));
        }

        [Test]
        public void FollowUserAsyncFollowsUser()
        {
            var user = context.UserProfiles.Find(DataSeeder.UserProfiles()[1].Id);

            var follower = context.UserProfiles.Find(DataSeeder.UserProfiles()[2].Id);

            var initialValue = context.UserProfiles.Find(user.Id).FollowedBy.Count;

            userActionsService.FollowUserAsync(user.Id, follower.Id);

            var actualValue = context.UserProfiles.Find(user.Id).FollowedBy.Count;

            Assert.IsTrue(initialValue < actualValue);
            Assert.IsTrue(initialValue + 1 == actualValue);
        }

        [Test]
        public void FollowUserAsyncSendsFollowRequestIfUserIsPrivate()
        {
            var privateUser = context.UserProfiles.Find(DataSeeder.UserProfiles()[0].Id);

            var follower = context.UserProfiles.Find(DataSeeder.UserProfiles()[2].Id);

            var initialValue = context.UserProfiles.Find(privateUser.Id).FollowRequests.Count;

            userActionsService.FollowUserAsync(privateUser.Id, follower.Id);

            var actualValue = context.UserProfiles.Find(privateUser.Id).FollowRequests.Count;

            Assert.IsTrue(initialValue < actualValue);
            Assert.IsTrue(initialValue + 1 == actualValue);
        }


        [Test]
        public void FollowUserAsyncDoesNotFollowPrivateAccounts()
        {
            var privateUser = context.UserProfiles.Find(DataSeeder.UserProfiles()[0].Id);
            var followerInitialCount = privateUser.FollowedBy.Count;

            var follower = context.UserProfiles.Find(DataSeeder.UserProfiles()[2].Id);

            userActionsService.FollowUserAsync(privateUser.Id, follower.Id);

            var followerActualCount = privateUser.FollowedBy.Count;

            Assert.AreEqual(followerInitialCount, followerActualCount);
        }

        [Test]
        public void UnfollowUserAsyncHandlesInvalidData()
        {
            var user = context.UserProfiles.Find(DataSeeder.UserProfiles()[0].Id);

            Assert.DoesNotThrowAsync(async () => await userActionsService.UnfollowUserAsync(user.Id, "fake-user"));
            Assert.DoesNotThrowAsync(async () => await userActionsService.UnfollowUserAsync("fake_user", user.Id));
            Assert.DoesNotThrowAsync(async () => await userActionsService.UnfollowUserAsync("fake_user", "fake-user"));
        }

        [Test]
        public void UnfollowUserAsyncUnfollowsUser()
        {
            var user = context.UserProfiles.Find(DataSeeder.UserProfiles()[0].Id);

            var userToBeUnfollowed = context.UserProfiles.Find(DataSeeder.UserProfiles()[1].Id);

            var initialValue = context.UserProfiles.Find(user.Id).Follows.Count;

            userActionsService.UnfollowUserAsync(userToBeUnfollowed.Id, user.Id);

            var actualValue = context.UserProfiles.Find(user.Id).Follows.Count;

            Assert.IsTrue(initialValue > actualValue);
            Assert.IsTrue(initialValue - 1 == actualValue);
        }

        [Test]
        public void ApproveFollowRequestAsyncHandlesInvalidData()
        {
            var user = context.UserProfiles.Find(DataSeeder.UserProfiles()[0].Id);

            Assert.DoesNotThrowAsync(async () => await userActionsService.ApproveFollowRequestAsync(user.Id, "fake-user"));
            Assert.DoesNotThrowAsync(async () => await userActionsService.ApproveFollowRequestAsync("fake_user", user.Id));
            Assert.DoesNotThrowAsync(async () => await userActionsService.ApproveFollowRequestAsync("fake_user", "fake-user"));
        }

        [Test]
        public void ApproveFollowRequestAsyncRemovesFollowRequset()
        {
            var user = context.UserProfiles.Find(DataSeeder.UserProfiles()[0].Id);
            var requester = context.UserProfiles.Find(DataSeeder.UserProfiles()[2].Id);

            var initialFollowRequestsCount = user.FollowRequests.Count;

            userActionsService.ApproveFollowRequestAsync(requester.Id, user.Id);

            var actualFollowRequestsCount = user.FollowRequests.Count;

            Assert.IsTrue(initialFollowRequestsCount > actualFollowRequestsCount);
            Assert.IsTrue(initialFollowRequestsCount - 1 == actualFollowRequestsCount);
        }

        [Test]
        public void ApproveFollowRequestAsyncAddsFollower()
        {
            var user = context.UserProfiles.Find(DataSeeder.UserProfiles()[0].Id);
            var requester = context.UserProfiles.Find(DataSeeder.UserProfiles()[3].Id); //does not follow user

            var initialFollowersCount = user.FollowedBy.Count;

            userActionsService.ApproveFollowRequestAsync(requester.Id, user.Id);

            var actualFollowRequestsCount = user.FollowedBy.Count;

            Assert.IsTrue(initialFollowersCount < actualFollowRequestsCount);
            Assert.IsTrue(initialFollowersCount + 1 == actualFollowRequestsCount);
        }

        [Test]
        public void DeleteRequestAsyncHandlesInvalidData()
        {
            var user = context.UserProfiles.Find(DataSeeder.UserProfiles()[0].Id);
                                                                                                                             //fake users
            Assert.DoesNotThrowAsync(async () => await userActionsService.DeleteRequestAsync("515e8228-47eb-49ee-a9c0-467071015c5d", "a07ac7fb-e808-47c1-8133-16dc9ac8ae79"));
            Assert.DoesNotThrowAsync(async () => await userActionsService.DeleteRequestAsync("fake_user", user.Id));
        }

        [Test]
        public void DeleteRequestAsyncDeletesFollowRequest()
        {
            var user = context.UserProfiles.Find(DataSeeder.UserProfiles()[0].Id);
            var requester = context.UserProfiles.Find(DataSeeder.UserProfiles()[4].Id); //User who has send request

            var initialRequestsCount = user.FollowRequests.Count;

            userActionsService.DeleteRequestAsync(requester.Id, user.Id);

            var actualFollowRequestsCount = user.FollowRequests.Count;

            Assert.IsTrue(initialRequestsCount > actualFollowRequestsCount);
            Assert.IsTrue(initialRequestsCount - 1 == actualFollowRequestsCount);

            userActionsService.FollowUserAsync(requester.Id, user.Id); //rearrange DB
        }

        [Test]
        public void RemoveFollowerAsyncHandlesInvalidData()
        {
            var user = context.UserProfiles.Find(DataSeeder.UserProfiles()[0].Id);

            Assert.DoesNotThrowAsync(async () => await userActionsService.RemoveFollowerAsync(user.Id, "fake-user"));
            Assert.DoesNotThrowAsync(async () => await userActionsService.RemoveFollowerAsync("fake_user", "fake-user"));
        }

        [Test]
        public void RemoveFollowerAsyncRemoveFollower()
        {
            var user = context.UserProfiles.Find(DataSeeder.UserProfiles()[0].Id); 
            var follower = context.UserProfiles.Find(DataSeeder.UserProfiles()[1].Id); //User who follows current user

            var initialFollowers = user.FollowedBy.Count;

            userActionsService.RemoveFollowerAsync(user.Id, follower.Id);

            var actualFollowersCount = user.FollowedBy.Count;

            Assert.IsTrue(initialFollowers > actualFollowersCount);
            Assert.IsTrue(initialFollowers - 1 == actualFollowersCount);
        }

        [Test]
        public void CheckIfFollowRequestIsSentReturnsCorrectData()
        {
            var user = context.UserProfiles.Find(DataSeeder.UserProfiles()[0].Id);
            var requester = context.UserProfiles.Find(DataSeeder.UserProfiles()[4].Id); //User who has send request

            Assert.IsTrue(userActionsService.CheckIfFollowRequestIsSent(user.Id, requester.Id));
            Assert.IsFalse(userActionsService.CheckIfFollowRequestIsSent(user.Id, "user who did not send request"));
        }

        [Test]
        public void IsUserFollowingAsyncHandlesInvalidData()
        {
            var user = context.UserProfiles.Find(DataSeeder.UserProfiles()[0].Id);

            Assert.DoesNotThrowAsync(async () => await userActionsService.IsUserFollowingAsync(user.Id, "fake-user"));
            Assert.DoesNotThrowAsync(async () => await userActionsService.IsUserFollowingAsync("fake__User", user.Id));
            Assert.DoesNotThrowAsync(async () => await userActionsService.IsUserFollowingAsync("fake_user", "fake-user"));
        }

        [Test]
        public void IsUserFollowingAsyncReturnsCorrectData()
        {
            var user = context.UserProfiles.Find(DataSeeder.UserProfiles()[0].Id); //Does not follow uesr[2]
            var followedUser = context.UserProfiles.Find(DataSeeder.UserProfiles()[2].Id); //Follows user[0]

            Assert.IsTrue(userActionsService.IsUserFollowingAsync(followedUser.Id, user.Id).Result);
            Assert.IsFalse(userActionsService.IsUserFollowingAsync(user.Id, followedUser.Id).Result);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }
    }
}
