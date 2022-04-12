using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SocialMedia1.Areas.Admin.Models;
using SocialMedia1.Areas.Admin.Services;
using SocialMedia1.Data;
using SocialMedia1.Tests.Data;
using System.Linq;

namespace SocialMedia1.Tests.Tests.Administration
{
    [TestFixture]
    public class AdministrationTests
    {
        public static DbContextOptions<ApplicationDbContext> dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
          .UseInMemoryDatabase("SocialMedia1Tests")
          .Options;


        private ApplicationDbContext context;
        private IReportService reportService;

        [OneTimeSetUp]
        public void SetUp()
        {
            context = new ApplicationDbContext(dbOptions);
            context.Database.EnsureCreated();

            DataSeeder.Seed(context);

            reportService = new ReportService(context);
        }

        [Test]
        public void GetAllReportedPostsAsyncReturnsUniquePosts()
        {
            var reports = context.PostCommunityReports.Select(x => x.PostId).Distinct().Count();

            var actual = reportService.GetAllReportedPostsAsync();

            Assert.AreEqual(reports, actual.Result.Count);
        }

        [Test]
        [TestCase(0)]
        public void GetAllReportedPostsAsyncReturnsCorrectData(int index)
        {
            var reports = context.PostCommunityReports.Select(x => x.PostId).Distinct().ToList();

            var actual = reportService.GetAllReportedPostsAsync().Result.ToList();

            Assert.AreEqual(reports[index], actual[index].PostId);
        }

        [Test]
        public void ApproveReportedPostAsyncHandlesIncorrectData()
        {
            Assert.DoesNotThrowAsync(async () => await reportService.ApproveReportedPostAsync("blabla"));
        }

        [Test]
        public void ApproveReportedPostAsyncShouldPostFromReportedPostsSection()
        {
            var post = DataSeeder.Posts()[1];

            var initialReportsCount = context.PostCommunityReports.CountAsync().Result;

            var reportsForCurrentPost = context.PostCommunityReports.Where(x => x.PostId == post.Id).Count();

            reportService.ApproveReportedPostAsync(post.Id).Wait();

            var actualCount = context.PostCommunityReports.CountAsync().Result;

            Assert.IsTrue(initialReportsCount == actualCount + reportsForCurrentPost);
            Assert.IsFalse(context.PostCommunityReports.AnyAsync(x => x.PostId == post.Id).Result);
        }

        [Test]
        public void DeleteReportedPostAsyncHadnlesInvalidData()
        {
            Assert.DoesNotThrowAsync(async () => await reportService.DeleteReportedPostAsync("!@#!#!@#"));
        }

        [Test]
        public void DeleteReportedPostAsyncMarksPostAsDeleted()
        {
            var post = DataSeeder.GroupPosts()[0];

            var initialValue = context.Posts.Find(post.Id).IsDeleted;

            reportService.DeleteReportedPostAsync(post.Id);

            var actualValue = context.Posts.Find(post.Id).IsDeleted;

            Assert.AreNotEqual(initialValue, actualValue);
            Assert.IsFalse(initialValue);
            Assert.IsTrue(actualValue);
        }

        [Test]
        public void DeleteReportedPostAsyncRemovesPostsFromReportedPostsSection()
        {
            var post = DataSeeder.GroupPosts()[1];

            var initialReportsCount = context.PostCommunityReports.CountAsync().Result;

            var reportsForCurrentPost = context.PostCommunityReports.Where(x => x.PostId == post.Id).Count();

            reportService.DeleteReportedPostAsync(post.Id).Wait();

            var actualCount = context.PostCommunityReports.CountAsync().Result;

            Assert.IsTrue(initialReportsCount == actualCount + reportsForCurrentPost);
            Assert.IsFalse(context.PostCommunityReports.AnyAsync(x => x.PostId == post.Id).Result);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }
    }
}
