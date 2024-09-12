using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SocialMedia1.Data;
using SocialMedia1.Services.Groups;
using SocialMedia1.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia1.Tests.Tests.GroupTests
{
    [TestFixture]
    public class GroupTests
    {
        private static DbContextOptions<ApplicationDbContext> dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("SocialMedia1Tests")
           .Options;


        private ApplicationDbContext context;
        private IGroupService groupService;

        [OneTimeSetUp]
        public void SetUp()
        {
            context = new ApplicationDbContext(dbOptions);
            context.Database.EnsureCreated();

            DataSeeder.Seed(context);

            groupService = new GroupService(context);
        }

        [Test]
        public void CreateGroupAsyncCreatesGroup()
        {
            string groupName = "Test masters";
            string groupDescription = "Unit tests";

            groupService.CreateGroupAsync(groupName, groupDescription, true, DataSeeder.UserProfiles()[0].Id);

            Assert.IsTrue(context.Groups.Any(x => x.Name == groupName && x.Description == groupDescription));
        }

        [Test]
        public void CreateGroupAsyncMakesUserCreaterMemberOfTheGroup()
        {
            var user = context.UserProfiles.Find(DataSeeder.UserProfiles()[0].Id);

            string groupName = "Сам съм си QA";
            string groupDescription = "...---...";

            groupService.CreateGroupAsync(groupName, groupDescription, true, DataSeeder.UserProfiles()[0].Id);

            var isCreatorGroupMember = context.UserProfilesGroups.Any(x => (x.Group.Name == groupName && x.Group.Description == groupDescription) && x.UserProfile.Id == user.Id);

            Assert.IsTrue(isCreatorGroupMember);
        }

        [Test]
        public void GetGroupHandlesInvalidData()
        {
            Assert.DoesNotThrow(() => groupService.GetGroup("unexisting group"));
        }

        [Test]
        public void GetGroupReturnCorrectModel()
        {
            var groupCreatorId = DataSeeder.UserProfiles()[0].Id;

            var expectedGroup = DataSeeder.Groups().First(x => x.CreaterId == groupCreatorId);

            var actualGroup = groupService.GetGroup(expectedGroup.Id);

            Assert.AreEqual(expectedGroup.Id, actualGroup.Id);
            Assert.AreEqual(expectedGroup.Name, actualGroup.Name);
            Assert.AreEqual(expectedGroup.Description, actualGroup.Description);
        }

        [Test]
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void GetGroupsReturnsSetOfUsersGroups(int userIndex)
        {
            var userId = DataSeeder.UserProfiles()[userIndex].Id;

            var expected = context.UserProfilesGroups.Where(x => x.UserProfileId == userId).Count();

            var actual = groupService.GetGroups(userId).Length;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetMembersAsyncHandlesInvalidData()
        {
            Assert.DoesNotThrowAsync(async () => await groupService.GetMembersAsync("invalid groupId"));

            var groupId = DataSeeder.Groups()[1].Id; //group with 3 members

            var expected = context.UserProfilesGroups.Where(x=>x.GroupId == groupId).Count();

            var actual = groupService.GetMembersAsync(groupId).Result.Members.Count;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void GetJoinGroupRequestsAsyncHandlesInvalidData()
        {
            Assert.DoesNotThrowAsync(async () => await groupService.GetJoinGroupRequestsAsync("invalid groupId"));
        }

        [Test]
        public void GetJoinGroupRequestsAsyncReturnsAllJoinRequestsForTheGroup()
        {
            var groupId = DataSeeder.Groups()[0].Id; //group with 2 join requests

            var expected = context.JoinGroupRequest.Where(x => x.GroupId == groupId).Count();

            var actual = groupService.GetJoinGroupRequestsAsync(groupId).Result.Count;

            Assert.AreEqual(expected, actual);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }
    }
}
