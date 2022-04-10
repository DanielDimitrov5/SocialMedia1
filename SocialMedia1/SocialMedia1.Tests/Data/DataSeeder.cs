using Microsoft.AspNetCore.Identity;
using SocialMedia1.Data;
using SocialMedia1.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia1.Tests.Data
{
    internal static class DataSeeder
    {
        private const string privateUserId = "fee859e9-7b26-4cf8-a3c5-7cea60c13101";

        public static List<UserProfile> UserProfiles()
        {
            List<UserProfile> users = new List<UserProfile>
            {
                new UserProfile
                {
                    Id = privateUserId, //1 private user
                    Nickname = "dani",
                    Name = "Daniel",
                    Surname="Dimitrov",
                    ImageUrl ="https://res.cloudinary.com/dani03/image/upload/v1649394244/b2021f98-e33e-4ca6-9f54-4adc7c205949.jpg",
                    EmailAddress = "dani@gamil.com",
                    Bio = "xD",
                    IsPrivate = true,
                },
                new UserProfile
                {
                    Id = "99c88001-1221-4ff7-a0e9-f5efac98fe9e", //2
                    Nickname = "ivan",
                    EmailAddress = "ivan@gamil.com",
                    Name = "Ivan",
                    Surname="Ivanov",
                    ImageUrl ="https://res.cloudinary.com/dani03/image/upload/v1649394244/b2021f98-e33e-4ca6-9f54-4adc7c205949.jpg",
                    Bio = "1337",
                    IsPrivate = false,
                },
                new UserProfile
                {
                    Id = "71b0265a-1994-4b4a-a824-8a4401aae60e", //3
                    Nickname = "stefan",
                    Name = "Stefan",
                    Surname="Stefanov",
                    ImageUrl ="https://res.cloudinary.com/dani03/image/upload/v1649394244/b2021f98-e33e-4ca6-9f54-4adc7c205949.jpg",
                    EmailAddress = "stefan@gamil.com",
                    Bio = "Bio",
                    IsPrivate = false,
                },
                new UserProfile
                {
                    Id = "b9a3f5d8-532d-49eb-b3f8-f0b4e95bb62e", //5
                    Nickname = "gosho",
                    Name = "Gosho",
                    Surname="Goshkov",
                    ImageUrl ="https://res.cloudinary.com/dani03/image/upload/v1649394244/b2021f98-e33e-4ca6-9f54-4adc7c205949.jpg",
                    EmailAddress = "gosho@gamil.com",
                    Bio = "Stay postive!",
                    IsPrivate = false,
                },
                new UserProfile
                {
                    Id = "e3749770-6d4e-4f3c-886b-13e953eafb50", //5
                    Nickname = "ivo",
                    Name = "Ivo",
                    Surname="Ivakov",
                    ImageUrl ="https://res.cloudinary.com/dani03/image/upload/v1649394244/b2021f98-e33e-4ca6-9f54-4adc7c205949.jpg",
                    EmailAddress = "ivo@gamil.com",
                    Bio = "mynameisivo",
                    IsPrivate = false,
                },
            };

            return users;
        }

        public static List<FollowRequest> FollowRequests()
        {
            var followRequests = new List<FollowRequest>
            {
                new FollowRequest
                {
                    UserId = UserProfiles()[0].Id,
                    UserRequesterId = UserProfiles()[2].Id,
                },
                new FollowRequest
                {
                    UserId = UserProfiles()[0].Id,
                    UserRequesterId = UserProfiles()[3].Id,
                },
                new FollowRequest
                {
                    UserId = UserProfiles()[0].Id,
                    UserRequesterId = UserProfiles()[4].Id,
                },
            };

            return followRequests;
        }

        public static IdentityUser User()
        {
            return new IdentityUser
            {
                Id = "81590b37-a922-4c95-8603-1caa40fba811",
                Email = "user123@abv.bg",
                UserName = "user123@abv.bg",
            };
        }

        public static Group[] Groups()
        {
            return new Group[]
            {
                new Group
                {
                    Id = "1ddd2e67-8b52-46d1-a82d-773f218d85d9",
                    Name = "Dani's group",
                    Description = "...---...",
                    CreaterId = UserProfiles()[0].Id,
                    IsPrivate = true,
                    MembersCount = 1,
                },
                new Group
                {
                    Id = "c4143e16-a477-41ec-9ef0-8be09b71a457",
                    Name = "Ivan's group",
                    Description = "Over 5 charcaters",
                    CreaterId = UserProfiles()[1].Id,
                    IsPrivate = false,
                    MembersCount = 1,
                },
            };
        }

        public static UserProfileGroup[] UserProfilesGroups()
        {
            return new UserProfileGroup[]
            {
                new UserProfileGroup
                {
                    GroupId = Groups()[1].Id,
                    UserProfileId = UserProfiles()[0].Id,
                },
                new UserProfileGroup
                {
                    GroupId = Groups()[1].Id,
                    UserProfileId = UserProfiles()[1].Id,
                },
                new UserProfileGroup
                {
                    GroupId = Groups()[1].Id,
                    UserProfileId = UserProfiles()[2].Id,
                },
            };
        }

        public static UserGroupRequest[] JoinGroupRequests()
        {
            return new UserGroupRequest[]
            {
                new UserGroupRequest
                {
                    GroupId = Groups()[0].Id,
                    UserProfileId = UserProfiles()[1].Id,
                },
                new UserGroupRequest
                {
                    GroupId = Groups()[0].Id,
                    UserProfileId = UserProfiles()[2].Id,
                },
            };
        }

        public static void Seed(ApplicationDbContext context)
        {
            context.Users.Add(User());

            var users = UserProfiles();

            context.UserProfiles.AddRange(users);

            users.First(x => x.Id == privateUserId).Follows.Add(users[1]);
            users.First(x => x.Id == privateUserId).FollowedBy.Add(users[1]);
            users.First(x => x.Id == privateUserId).FollowedBy.Add(users[2]);

            var privateUser = users[0];

            foreach (var req in FollowRequests())
            {
                privateUser.FollowRequests.Add(req);
            }

            context.Groups.AddRange(Groups());

            context.JoinGroupRequest.AddRange(JoinGroupRequests());

            context.UserProfilesGroups.AddRange(UserProfilesGroups());

            context.SaveChanges();
        }
    }
}
