using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SocialMedia1.Data;
using SocialMedia1.Models.Posts;
using SocialMedia1.Services.Posts;
using SocialMedia1.Services.Users;
using SocialMedia1.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia1.Tests.Tests.PostTests
{
    [TestFixture]
    public class PostTests
    {
        public static DbContextOptions<ApplicationDbContext> dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseInMemoryDatabase("SocialMedia1Tests")
          .Options;


        private ApplicationDbContext context;
        private IPostService postService;

        [OneTimeSetUp]
        public void SetUp()
        {
            context = new ApplicationDbContext(dbOptions);
            context.Database.EnsureCreated();

            DataSeeder.Seed(context);

            postService = new PostService(context);
        }

        [Test]
        public void CreatePostAsyncHandlesInvalidData()
        {
            Assert.DoesNotThrowAsync(async () => await postService.CreatePostAsync("invalid user Id", ":-)"));
        }

        [Test]
        public void CreatePostAsyncCreatesPost()
        {
            var user = DataSeeder.UserProfiles()[0];

            var content = "test post";

            postService.CreatePostAsync(user.Id, content);

            Assert.True(context.Posts.Any(x => x.UserProfileId == user.Id && x.Content == content));
        }

        [Test]
        public void CreateGroupPostAsyncHandlesInvalidData()
        {
            Assert.DoesNotThrowAsync(() => postService.CreateGroupPostAsync("fake", "fake", "fake"));
        }

        [Test]
        public void CreateGroupPostAsyncAddPostIntoGroups()
        {
            var user = DataSeeder.UserProfiles()[0];

            var group = DataSeeder.Groups()[0];

            var content = "funny content";

            postService.CreateGroupPostAsync(group.Id, user.Id, content);

            Assert.True(context.Posts.Any(x => x.UserProfileId == user.Id && x.GroupId == group.Id && x.Content == content));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetAllPostsRetrunsAllPostFromUserWhichAreNotDeleted(int index)
        {
            var user = DataSeeder.UserProfiles()[0];

            var expected = context.Posts.Where(x => x.UserProfileId == user.Id && !x.IsDeleted).Select(x => new PostViewModel
            {
                Author = x.UserProfile.Nickname,
                AuthorId = x.UserProfile.Id,
                ImageUrl = x.UserProfile.ImageUrl,
                Id = x.Id,
                Content = x.Content,
                CreatedOn = x.CreatedOn,
                GroupId = x.GroupId,
            }).ToList();

            var actual = postService.GetAllPosts(user.Id);

            Assert.AreEqual(expected[index].Id, actual.ToArray()[index].Id);
            Assert.AreEqual(expected[index].GroupId, actual.ToArray()[index].GroupId);
            Assert.AreEqual(expected[index].AuthorId, actual.ToArray()[index].AuthorId);
        }

        [Test]
        public void GetAllPostsByFollowedUsersHandlesInvalidData()
        {
            Assert.DoesNotThrow(() => postService.GetAllPostsByFollowedUsers("djadjdsjs"));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void GetAllPostsByFollowedUsersReturnsPosts(int index)
        {
            var user = DataSeeder.UserProfiles()[1];

            var expected = context.UserProfiles.Where(x => x.FollowedBy.Contains(user))
            .SelectMany(x => x.Posts)
            .ToList()
            .Where(x => x.GroupId is null && !x.IsDeleted)
            .Select(x => new PostViewModel
            {
                AuthorId = x.UserProfile.Id,
                ImageUrl = x.UserProfile.ImageUrl,
                Content = x.Content,
                Id = x.Id,
                CreatedOn = x.CreatedOn,
            })
            .ToList(); // 2 posts(groups not included)

            var actual = postService.GetAllPostsByFollowedUsers(user.Id);

            Assert.AreEqual(expected[index].Id, actual.ToArray()[index].Id);
            Assert.AreEqual(expected[index].GroupId, actual.ToArray()[index].GroupId);
            Assert.AreEqual(expected[index].AuthorId, actual.ToArray()[index].AuthorId);
        }

        [Test]
        public void GetAllPostsInUsersGroupsHandlesInvalidData()
        {
            Assert.DoesNotThrow(() => postService.GetAllPostsInUsersGroups("fakekekeke"));
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        public void GetAllPostsInUsersGroupsReturnsAllGroupPostsUsersIsMemberOf(int index)
        {
            var user = DataSeeder.UserProfiles()[4];

            var expected = context.Groups.Where(x => x.Users.Any(x => x.UserProfileId == user.Id))
                .SelectMany(x => x.Posts)
                .ToList()
                .Where(x => !x.IsDeleted)
                .Select(x => new PostViewModel
                {
                    AuthorId = x.UserProfile.Id,
                    ImageUrl = x.UserProfile.ImageUrl,
                    Content = x.Content,
                    Id = x.Id,
                    CreatedOn = x.CreatedOn,
                    GroupId = x.GroupId,
                })
                .ToList(); // 2 posts(only groups)

            var actual = postService.GetAllPostsInUsersGroups(user.Id);

            Assert.AreEqual(expected[index].Id, actual.ToArray()[index].Id);
            Assert.AreEqual(expected[index].GroupId, actual.ToArray()[index].GroupId);
            Assert.AreEqual(expected[index].AuthorId, actual.ToArray()[index].AuthorId);
        }

        [Test]
        public void DeletePostAsyncHandlesInvalidData()
        {
            Assert.DoesNotThrowAsync(async () => await postService.DeletePostAsync("dada", "nene"));
        }

        [Test]
        public void DeletePostAsyncMarkPostAsDeleted()
        {
            var post = DataSeeder.Posts()[0]; //IsDeleted = 0

            var userId = post.UserProfileId;

            var initialPostCount = context.Posts.Count();

            postService.DeletePostAsync(post.Id, userId);

            var actualPostCount = context.Posts.Count();

            Assert.AreEqual(initialPostCount, actualPostCount);
            Assert.IsTrue(context.Posts.Find(post.Id).IsDeleted);
        }

        [Test]
        public void ReportPostAsyncHandlesInvalidData()
        {
            Assert.DoesNotThrowAsync(() => postService.ReportPostAsync("invalid id", "invalid user"));
        }

        [Test]
        public void ReportPostAsyncDoesNothingIfReportIsAlreadySent()
        {
            var report = DataSeeder.PostCommunityReport();

            var IntialreportsCount = context.PostCommunityReports.Count();

            postService.ReportPostAsync(report.PostId, report.ReporterId);

            var actualReportsCount = context.PostCommunityReports.Count();

            Assert.AreEqual(IntialreportsCount, actualReportsCount);
        }

        [Test]
        public void ReportPostAsyncShouldSendReports()
        {
            var post = DataSeeder.Posts()[1];
            var user = DataSeeder.UserProfiles()[1];

            var IntialreportsCount = context.PostCommunityReports.Count();

            postService.ReportPostAsync(post.Id, user.Id);

            var actualReportsCount = context.PostCommunityReports.Count();

            Assert.IsTrue(IntialreportsCount == actualReportsCount - 1);
            Assert.IsTrue(context.PostCommunityReports.Any(x => x.PostId == post.Id && x.ReporterId == user.Id));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }
    }
}
