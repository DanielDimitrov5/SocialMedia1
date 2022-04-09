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
                    UserRequesterId = UserProfiles()[1].Id,
                },
                new FollowRequest
                {
                    UserId = UserProfiles()[0].Id,
                    UserRequesterId = UserProfiles()[2].Id,
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

        public static void Seed(ApplicationDbContext context)
        {
            context.Users.Add(User());

            var users = UserProfiles();

            users.First(x => x.Id == privateUserId).Follows.Add(users[1]);
            users.First(x => x.Id == privateUserId).FollowedBy.Add(users[1]);

            context.UserProfiles.AddRange(users);

            var privateUser = users[0];

            foreach (var req in FollowRequests())
            {
                privateUser.FollowRequests.Add(req);
            }

            context.SaveChanges();
        }
    }
}
