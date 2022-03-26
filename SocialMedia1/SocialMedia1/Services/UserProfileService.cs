using SocialMedia1.Data;
using SocialMedia1.Data.Models;
using SocialMedia1.Models;
using Microsoft.AspNetCore.Identity;


namespace SocialMedia1.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly ApplicationDbContext context;
        private readonly IPostService postService;
        private readonly UserManager<IdentityUser> userManager;

        public UserProfileService(ApplicationDbContext context, IPostService postService, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.postService = postService;
            this.userManager = userManager;
        }

        public async Task AddUserProfileAsync(string Id)
        {
            string email = context.Users.FirstOrDefault(x => x.Id == Id).Email;

            string nickname = email.Split('@')[0];

            await context.UserProfiles.AddAsync(new UserProfile { Id = Id, Nickname = nickname, EmailAddress = email });

            await context.SaveChangesAsync();
        }

        public async void EditUserProfileAsync(string id, string nickname, string name, string surename, bool IsPrivate, string city, DateTime birthday, string emailaddress, string bio)
        {
            UserProfile userProfile = context.UserProfiles.First(x => x.Id == id);

            userProfile.Nickname = nickname;
            userProfile.Name = name;
            userProfile.Surname = surename;
            userProfile.IsPrivate = IsPrivate;
            userProfile.City = city;
            userProfile.Birthday = birthday;
            userProfile.EmailAddress = emailaddress;
            userProfile.Bio = bio;

            context.SaveChanges();
        }

        public ProfileViewModel GetUserProfileData(string id)
        {
            UserProfile user = context.UserProfiles.FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                return null;
            }

            context.Entry(user).Collection(x => x.FollowedBy).Load();
            context.Entry(user).Collection(x => x.Follows).Load();

            var posts = postService.GetAllPosts(id);

            ProfileViewModel model = new ProfileViewModel
            {
                Id = user.Id,
                Nickname = user.Nickname,
                Name = user.Name,
                Surname = user.Surname,
                City = user.City,
                Age = user.Age,
                Birthday = user.Birthday,
                EmailAddress = user.EmailAddress,
                Bio = user.Bio,
                Posts = posts,
                FollowersCount = user.FollowedBy.Count(),
                FollowingCount = user.Follows.Count(),
            };

            return model;
        }

        public ICollection<FollowRequestViewModel> GetAllFollowRequests(string currentUserId)
        {
            return context.FollowRequests.Where(x => x.UserId == currentUserId).Select(x => new FollowRequestViewModel
            {
                RequesterId = x.UserRequester.Id,
                Nickname = x.UserRequester.Nickname,
                CurrentUserId = currentUserId,
            }).ToList();
        }

        public UsersProfilesViewModel GetAllFollowers(string currentUserId)
        {
            var followers = context.UserProfiles.Where(x => x.Follows.Any(x => x.Id == currentUserId))
                .Select(x => new ProfileViewModel
                {
                    Id = x.Id,
                    Nickname = x.Nickname,
                }).ToList();

            UsersProfilesViewModel model = new UsersProfilesViewModel
            {
                Id = currentUserId,
                Profiles = followers,
            };

            return model;
        }

        public UsersProfilesViewModel GetAllFollowing(string currentUserId)
        {
            var user = context.UserProfiles.Find(currentUserId);

            context.Entry(user).Collection(x => x.Follows).Load();

            var following = user.Follows
                 .Select(x => new ProfileViewModel
                 {
                     Id = x.Id,
                     Nickname = x.Nickname,
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
