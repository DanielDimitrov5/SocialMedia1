using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SocialMedia1.Data;
using SocialMedia1.Services.Groups;
using SocialMedia1.Tests.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SocialMedia1.Tests.Tests.GroupTests
{
    public class GroupMemberActionsTests
    {
        public static DbContextOptions<ApplicationDbContext> dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
           .UseInMemoryDatabase("SocialMedia1Tests")
           .Options;


        private ApplicationDbContext context;
        private IGroupMemberActionsService groupMemberActionsService;

        [OneTimeSetUp]
        public void SetUp()
        {
            context = new ApplicationDbContext(dbOptions);
            context.Database.EnsureCreated();

            DataSeeder.Seed(context);

            groupMemberActionsService = new GroupMemberActionsService(context);
        }

        //GO!

        [OneTimeTearDown]
        public void CleanUp()
        {
            context.Database.EnsureDeleted();
        }
    }
}
