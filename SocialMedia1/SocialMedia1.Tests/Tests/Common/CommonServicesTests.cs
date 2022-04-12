using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SocialMedia1.Data;
using SocialMedia1.Models.Common;
using SocialMedia1.Models.Groups;
using SocialMedia1.Models.Users;
using SocialMedia1.Services.Common;
using SocialMedia1.Services.Posts;
using SocialMedia1.Tests.Data;
using System.Linq;

namespace SocialMedia1.Tests.Tests.Common
{
    [TestFixture]
    public class CommonServicesTests
    {
        public static DbContextOptions<ApplicationDbContext> dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("SocialMedia1Tests")
           .Options;


        private ApplicationDbContext context;
        private ISearchService searchService;
        private INavBarService navBarService;
        private IIndexService indexService;

        private IPostService postService;

        [OneTimeSetUp]
        public void SetUp()
        {
            context = new ApplicationDbContext(dbOptions);
            context.Database.EnsureCreated();

            DataSeeder.Seed(context);

            searchService = new SearchService(context);
            navBarService = new NavBarService(context);

            postService = new PostService(context);

            indexService = new IndexService(postService);
        }

        [Test]
        [TestCase("group")] // should retrun both group
        [TestCase("...---...")] //dani's group description
        public void GetGroupsBySearchTermReturnsCorrectModel(string searchTerm)
        {
            var expected = context.Groups
               .Where(x => x.Name.Contains(searchTerm)
                        || x.Description.Contains(searchTerm)
                        || (x.Name + x.Description).Contains(searchTerm))
               .Select(x => new GroupViewModel
               {
                   Id = x.Id,
                   Name = x.Name,
                   Description = x.Description,
                   Members = x.MembersCount,
                   Status = x.IsPrivate ? "Private" : "Public",
               })
               .ToList();

            var actual = searchService.GetGroupsBySearchTerm(searchTerm).Result;

            Assert.AreEqual(expected.Select(x => x.Id), actual.Select(x => x.Id));
        }

        [Test]
        [TestCase("dani")]
        [TestCase("ov")] // DimitrOV, StefanOV, IvanOV
        public void GetProfilesBySearchTermReturnsCorrectModel(string searchTerm)
        {
            var expected = context.UserProfiles
                .Where(x => x.Nickname.Contains(searchTerm)
                         || x.Name.Contains(searchTerm)
                         || x.Surname.Contains(searchTerm)
                         || (x.Name + x.Surname).Contains(searchTerm))
                .Select(x => new ProfileViewModel
                {
                    Id = x.Id,
                    Nickname = x.Nickname,
                    Name = x.Name,
                    Surname = x.Surname,
                    Bio = x.Bio,
                    ImageUrl = x.ImageUrl,
                })
                .ToList();

            var actual = searchService.GetProfilesBySearchTerm(searchTerm).Result;

            Assert.AreEqual(expected.Select(x => x.Id), actual.Select(x => x.Id));
        }

        [Test]
        [TestCase(0)] //3
        [TestCase(1)] //0
        public void FollowRequestsCountRetunrsCorrectValue(int index)
        {
            var user = DataSeeder.UserProfiles()[index];

            var expected = context.UserProfiles.Find(user.Id).FollowRequests.Count;

            var actual = navBarService.FollowRequestsCount(user.Id);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(4)]
        public void GetIndexViewReturnsCorrectModel(int index)
        {
            var user = DataSeeder.UserProfiles()[index];

            var allPosts = postService.GetAllPosts(user.Id).Where(x => x.GroupId == null).ToList();

            var postsByFollowedUsers = postService.GetAllPostsByFollowedUsers(user.Id);

            allPosts.AddRange(postsByFollowedUsers);

            var expected = new IndexViewModel
            {
                Posts = allPosts,
                GroupPosts = postService.GetAllPostsInUsersGroups(user.Id),
            };

            var actual = indexService.GetIndexView(user.Id);

            Assert.AreEqual(expected.Posts.Count, actual.Posts.Count);
            Assert.AreEqual(expected.GroupPosts.Count, actual.GroupPosts.Count);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }
    }
}
