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
                new UserProfile
                {
                    Id = "57a00bd5-a366-4e57-bfb1-e45efa1ebe75", //6
                    Nickname = "stanimor",
                    Name = "Stancho",
                    Surname="Stankov",
                    ImageUrl ="https://res.cloudinary.com/dani03/image/upload/v1649394244/b2021f98-e33e-4ca6-9f54-4adc7c205949.jpg",
                    EmailAddress = "stancho@gamil.com",
                    Bio = "-",
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

                new UserProfileGroup
                {
                    GroupId = Groups()[0].Id,
                    UserProfileId = UserProfiles()[4].Id,
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
                new UserGroupRequest
                {
                    GroupId = Groups()[0].Id,
                    UserProfileId = UserProfiles()[3].Id,
                },
                new UserGroupRequest
                {
                    GroupId = Groups()[0].Id,
                    UserProfileId = UserProfiles()[4].Id,
                },
            };
        }

        public static Post[] Posts()
        {
            return new Post[]
            {
                new Post
                {
                    Id = "ea229b0f-5de4-48ef-919a-49c72f0597a4",
                    UserProfileId = UserProfiles()[0].Id,
                    Content = "cleaver post",
                    CreatedOn = DateTime.UtcNow.AddDays(-200),
                    GroupId = null,
                    IsDeleted = false,
                },
                new Post
                {
                    Id = "5278d247-b231-432a-a34b-b4d18687983c",
                    UserProfileId = UserProfiles()[0].Id,
                    Content = "dont know what to say",
                    CreatedOn = DateTime.UtcNow.AddDays(-250),
                    GroupId = null,
                    IsDeleted = false,
                },
            };
        }

        public static Post[] GroupPosts()
        {
            return new Post[]
            {
                new Post
                {
                    Id = "90a06326-dd3c-44ad-a78e-7b16067b0f05",
                    UserProfileId = UserProfiles()[0].Id,
                    Content = "cleaver group post",
                    CreatedOn = DateTime.UtcNow.AddDays(-40),
                    GroupId = Groups()[0].Id,
                    IsDeleted = false,
                },
                new Post
                {
                    Id = "ff3b6c77-8da0-4663-abf9-0c5020365e21",
                    UserProfileId = UserProfiles()[0].Id,
                    Content = "dont know what to say in this group",
                    CreatedOn = DateTime.UtcNow.AddDays(-10),
                    GroupId = Groups()[0].Id,
                    IsDeleted = false,
                },
            };
        }

        public static PostCommunityReport[] PostCommunityReports()
        {
            return new PostCommunityReport[]
            {
                //Report 0
                new PostCommunityReport
                {
                    Id = "25820efd-c510-495b-8c12-4dbf77d8b57b",
                    PostId = Posts()[0].Id,
                    ReporterId = UserProfiles()[1].Id,
                },

                //REPORT 1
                new PostCommunityReport
                {
                    Id = "e5ec0a03-ec1a-4a58-8a75-33797692a7c6",
                    PostId = Posts()[1].Id,
                    ReporterId = UserProfiles()[0].Id,
                },
                new PostCommunityReport
                {
                    Id = "cccc7155-32d3-472b-a643-708256f30307",
                    PostId = Posts()[1].Id,
                    ReporterId = UserProfiles()[2].Id,
                },
                new PostCommunityReport
                {
                    Id = "9f8805de-5a6d-476c-bede-ee21dc608328",
                    PostId = Posts()[1].Id,
                    ReporterId = UserProfiles()[3].Id,
                },
                new PostCommunityReport
                {
                    Id = "9889bdf9-cd4c-401b-b04e-9b4ddc4820e3",
                    PostId = Posts()[0].Id,
                    ReporterId = UserProfiles()[2].Id,
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

            context.SaveChanges();

            var privateUser = users[0];

            foreach (var req in FollowRequests())
            {
                privateUser.FollowRequests.Add(req);
            }

            context.SaveChanges();

            context.Groups.AddRange(Groups());

            context.JoinGroupRequest.AddRange(JoinGroupRequests());

            context.UserProfilesGroups.AddRange(UserProfilesGroups());

            context.SaveChanges();

            context.Posts.AddRange(Posts());
            context.Posts.AddRange(GroupPosts());

            context.PostCommunityReports.AddRange(PostCommunityReports());

            context.SaveChanges();
        }
    }
}
