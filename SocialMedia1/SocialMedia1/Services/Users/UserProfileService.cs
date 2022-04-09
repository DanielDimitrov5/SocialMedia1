using SocialMedia1.Data;
using SocialMedia1.Data.Models;
using SocialMedia1.Models;
using Microsoft.AspNetCore.Identity;
using SocialMedia1.Services.Posts;
using SocialMedia1.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace SocialMedia1.Services.Users
{
    public class UserProfileService : IUserProfileService
    {
        private readonly ApplicationDbContext context;
        private readonly IPostService postService;

        public UserProfileService(ApplicationDbContext context, IPostService postService)
        {
            this.context = context;
            this.postService = postService;
        }

        public async Task AddUserProfileAsync(string Id)
        {
            string email = context.Users.FirstOrDefault(x => x.Id == Id).Email;

            string nickname = email.Split('@')[0];

            await context.UserProfiles.AddAsync(new UserProfile { Id = Id, Nickname = nickname, EmailAddress = email });

            await context.SaveChangesAsync();
        }

        public async Task EditUserProfileAsync(string id, string nickname, string name, string surename, bool IsPrivate, string bio, string image)
        {
            UserProfile userProfile = context.UserProfiles.First(x => x.Id == id);

            userProfile.Nickname = nickname;
            userProfile.Name = name;
            userProfile.Surname = surename;
            userProfile.IsPrivate = IsPrivate;
            userProfile.Bio = bio;

            if (image != null)
            {
                userProfile.ImageUrl = image;
            }

            await context.SaveChangesAsync();
        }

        public async Task<ProfileViewModel> GetUserProfileDataAsync(string id)
        {
            UserProfile user = context.UserProfiles.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return null;
            }

            await context.Entry(user).Collection(x => x.FollowedBy).LoadAsync();
            await context.Entry(user).Collection(x => x.Follows).LoadAsync();

            var posts = postService.GetAllPosts(id).Where(x => x.GroupId == null).ToList();
            var groupPosts = postService.GetAllPosts(id).Where(x => x.GroupId != null).ToList();

            ProfileViewModel model = new ProfileViewModel
            {
                Id = user.Id,
                Nickname = user.Nickname,
                Name = user.Name,
                Surname = user.Surname,
                Bio = user.Bio,
                Posts = posts,
                FollowersCount = user.FollowedBy.Count,
                FollowingCount = user.Follows.Count,
                GroupPosts = groupPosts,
                IsPrivate = user.IsPrivate,
                ImageUrl = user.ImageUrl,
            };

            return model;
        }

        public async Task<ICollection<FollowRequestViewModel>> GetAllFollowRequests(string currentUserId)
        {
            return await context.FollowRequests.Where(x => x.UserId == currentUserId).Select(x => new FollowRequestViewModel
            {
                RequesterId = x.UserRequester.Id,
                Nickname = x.UserRequester.Nickname,
                Name = x.User.Name + " " + x.User.Surname,
                ImageUrl = x.UserRequester.ImageUrl,
                Bio = x.UserRequester.Bio,
                CurrentUserId = currentUserId,
            }).ToListAsync();
        }

        public UsersProfilesViewModel GetAllFollowers(string currentUserId)
        {
            var followers = context.UserProfiles.Where(x => x.Follows.Any(x => x.Id == currentUserId))
                .Select(x => new ProfileViewModel
                {
                    Id = x.Id,
                    Nickname = x.Nickname,
                    Name = x.Name + " " + x.Surname,
                    ImageUrl = x.ImageUrl,
                    Bio = x.Bio,
                }).ToList();

            UsersProfilesViewModel model = new UsersProfilesViewModel
            {
                Id = currentUserId,
                Profiles = followers,
            };

            return model;
        }

        public async Task<UsersProfilesViewModel> GetAllFollowingAsync(string currentUserId)
        {
            var user = await context.UserProfiles.FindAsync(currentUserId);

            await context.Entry(user).Collection(x => x.Follows).LoadAsync();

            var following = user.Follows
                 .Select(x => new ProfileViewModel
                 {
                     Id = x.Id,
                     Nickname = x.Nickname,
                     Name = x.Name + " " + x.Surname,
                     ImageUrl = x.ImageUrl,
                     Bio = x.Bio,
                 }).ToList();

            UsersProfilesViewModel model = new UsersProfilesViewModel
            {
                Id = currentUserId,
                Profiles = following,
            };

            return model;
        }
    }
}
