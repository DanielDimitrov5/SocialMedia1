using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SocialMedia1.Data;
using SocialMedia1.Services.Groups;
using SocialMedia1.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMedia1.Tests.Tests.GroupTests
{
    public class GroupMemberActionsTests
    {
        private static DbContextOptions<ApplicationDbContext> dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("SocialMedia1Tests")
           .Options;


        private ApplicationDbContext context;
        private IGroupMemberActionsService groupMemberActionsService;

        [OneTimeSetUp]
        public async Task SetUp()
        {
            context = new ApplicationDbContext(dbOptions);
            await context.Database.EnsureCreatedAsync();

            await DataSeeder.SeedAsync(context);

            groupMemberActionsService = new GroupMemberActionsService(context);
        }

        [Test]
        public void ApproveJoinRequestAsyncHandlesInvalidData()
        {
            Assert.DoesNotThrowAsync(async () => await groupMemberActionsService.ApproveJoinRequestAsync("fake-fake", DataSeeder.Groups()[0].Id));
            Assert.DoesNotThrowAsync(async () => await groupMemberActionsService.ApproveJoinRequestAsync("fake-fake", "_fake"));
        }

        [Test]
        public void ApproveJoinRequestAsyncAddUserToGroup()
        {
            var groupId = DataSeeder.Groups()[0].Id; // group with 4 requests

            var request = context.Groups.Find(groupId).JoinRequests.First();

            var initialGroupMembers = context.Groups.Find(groupId).MembersCount;

            groupMemberActionsService.ApproveJoinRequestAsync(request.UserProfileId, request.GroupId);

            var actualGroupMembers = context.Groups.Find(groupId).MembersCount;

            Assert.IsTrue(initialGroupMembers < actualGroupMembers);
            Assert.IsTrue(initialGroupMembers + 1 == actualGroupMembers);
        }

        [Test]
        public void ApproveJoinRequestAsyncRemovesJoinGroupRequest()
        {
            var groupId = DataSeeder.Groups()[0].Id; // group with 3 requests

            var initialJoinRequestsCount = context.Groups.Find(groupId).JoinRequests.Count();

            var request = context.Groups.Find(groupId).JoinRequests.First();

            groupMemberActionsService.ApproveJoinRequestAsync(request.UserProfileId, request.GroupId);

            var actualRequestsCount = context.Groups.Find(groupId).JoinRequests.Count();

            Assert.IsTrue(initialJoinRequestsCount > actualRequestsCount);
            Assert.IsTrue(initialJoinRequestsCount - 1 == actualRequestsCount);
        }

        [Test]
        public void DeleteJoinRequestAsyncHandlesInvalidData()
        {
            Assert.DoesNotThrowAsync(async () => await groupMemberActionsService.DeleteJoinRequestAsync("fake-fake", DataSeeder.Groups()[0].Id));
            Assert.DoesNotThrowAsync(async () => await groupMemberActionsService.DeleteJoinRequestAsync("fake-fake", "_fake"));
        }

        [Test]
        public void DeleteJoinRequestAsyncDeletesRequest()
        {
            var groupId = DataSeeder.Groups()[0].Id; // group with 2 requests

            var initialJoinRequestsCount = context.Groups.Find(groupId).JoinRequests.Count();

            var request = context.Groups.Find(groupId).JoinRequests.First();

            groupMemberActionsService.DeleteJoinRequestAsync(request.UserProfileId, request.GroupId);

            var actualRequestsCount = context.Groups.Find(groupId).JoinRequests.Count();

            Assert.IsTrue(initialJoinRequestsCount > actualRequestsCount);
            Assert.IsTrue(initialJoinRequestsCount - 1 == actualRequestsCount);
        }

        [Test]
        [TestCase(0)] // private
        [TestCase(1)] // public
        public async Task IsGroupPrivateReturnsCorrectResponse(int index)
        {
            var groupId = DataSeeder.Groups()[0].Id;

            var actual = context.Groups.Find(groupId).IsPrivate;

            var expected = await groupMemberActionsService.IsGroupPrivateAsync(groupId);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public async Task IsJoinRequestSentReturnsCorrectData()
        {
            var requesterId = DataSeeder.JoinGroupRequests().First().UserProfileId;

            var groupId = DataSeeder.JoinGroupRequests().First().GroupId;

            var expected = context.JoinGroupRequest.Any(x => x.UserProfileId == requesterId && x.GroupId == groupId);

            var actual = await groupMemberActionsService.IsJoinRequstSentAsync(groupId, requesterId);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void IsUserGroupCreatorReturnsCorrectData()
        {
            var group = DataSeeder.Groups().First();

            var creatorId = context.Groups.Find(group.Id).CreaterId;

            var actualCreatorId = groupMemberActionsService.IsUserGroupCreator(creatorId, group.Id);

            Assert.IsTrue(actualCreatorId);
        }

        [Test]
        public void IsUserGroupMemberReturnsCorrectData()
        {
            var group = DataSeeder.Groups()[1];

            var user = DataSeeder.UserProfiles()[1]; // group member

            Assert.IsTrue(groupMemberActionsService.IsUserGroupMember(user.Id, group.Id));
        }

        [Test]
        public void JoinGroupAsyncHandlesIncorrectData()
        {
            Assert.DoesNotThrowAsync(async () => await groupMemberActionsService.JoinGroupAsync(DataSeeder.Groups()[0].Id, "fake ID"));
            Assert.DoesNotThrowAsync(async () => await groupMemberActionsService.JoinGroupAsync("fake-fake", "_fake"));
        }

        [Test]
        public void JoinGroupAsyncAddUserToGroup()
        {
            var group = DataSeeder.Groups()[0];

            var user = DataSeeder.UserProfiles()[3];

            var initialGroupMembersCount = context.Groups.Find(group.Id).MembersCount;

            groupMemberActionsService.JoinGroupAsync(group.Id, user.Id);

            var actualGroupMembersCount = context.Groups.Find(group.Id).MembersCount;

            Assert.IsTrue(initialGroupMembersCount + 1 == actualGroupMembersCount);
        }

        [Test]
        public void LeaveGroupAsyncHandlesInvalidData()
        {
            Assert.DoesNotThrowAsync(async () => await groupMemberActionsService.LeaveGroupAsync(DataSeeder.Groups()[0].Id, "FAKE"));
            Assert.DoesNotThrowAsync(async () => await groupMemberActionsService.LeaveGroupAsync("fake-fake", "_fake"));
        }

        [Test]
        public void LeaveGroupAsyncRemovesUserFromGroup()
        {
            var group = DataSeeder.Groups()[1];
            var user = DataSeeder.UserProfiles()[0];

            var initialGroupMembers = context.Groups.Find(group.Id).MembersCount;

            groupMemberActionsService.LeaveGroupAsync(group.Id, user.Id);

            var actualGroupMembersCount = context.Groups.Find(group.Id).MembersCount;

            Assert.IsTrue(initialGroupMembers == actualGroupMembersCount + 1);
            Assert.IsFalse(context.Groups.Find(group.Id).Users.Any(x => x.UserProfileId == user.Id));
        }

        [Test]
        public void SendJoinRequestAsyncHandlesInvalidData()
        {
            Assert.DoesNotThrowAsync(async () => await groupMemberActionsService.SendJoinRequestAsync(DataSeeder.Groups()[0].Id, "fakefake"));
            Assert.DoesNotThrowAsync(async () => await groupMemberActionsService.SendJoinRequestAsync("fake-fake", "_fake"));
        }

        [Test]
        public void SendJoinRequestAsyncSendsJoinRequestToGroup()
        {
            var group = DataSeeder.Groups()[0];

            var user = DataSeeder.UserProfiles()[5];

            var initialGroupJoinRequests = context.Groups.Find(group.Id).JoinRequests.Count;

            groupMemberActionsService.SendJoinRequestAsync(group.Id, user.Id);

            var actualGroupJoinRequests = context.Groups.Find(group.Id).JoinRequests.Count;

            Assert.IsTrue(initialGroupJoinRequests == actualGroupJoinRequests - 1);
            Assert.IsTrue(context.Groups.Find(group.Id).JoinRequests.Any(x => x.UserProfileId == user.Id));
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }
    }
}
