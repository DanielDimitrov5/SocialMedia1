using SocialMedia1.Models;
using SocialMedia1.Models.Common;
using SocialMedia1.Models.Posts;
using SocialMedia1.Services.Posts;

namespace SocialMedia1.Services.Common
{
    public class IndexService : IIndexService
    {
        private readonly IPostService postService;

        public IndexService(IPostService postService)
        {
            this.postService = postService;
        }

        public IndexViewModel GetIndexView(string userId)
        {
            var postsByFollowedUsers = postService.GetAllPostsByFollowedUsers(userId);
            var myPosts = postService.GetAllPosts(userId).Where(x => x.GroupId == null).ToList();

            var allPosts = new List<PostViewModel>();

            allPosts.AddRange(postsByFollowedUsers);
            allPosts.AddRange(myPosts);

            return new IndexViewModel
            {
                Posts = allPosts,
                GroupPosts = postService.GetAllPostsInUsersGroups(userId),
            };
        }


        //TODO: Tests
        public string TimeSpanCalculator(DateTime dateTime)
        {
            var localTime = dateTime.ToLocalTime();

            var now = DateTime.Now;

            TimeSpan timeSpan = now - localTime;

            if (timeSpan.TotalSeconds < 60)
            {
                return "now";
            }
            else if (timeSpan.TotalMinutes < 60)
            {
                string m = timeSpan.Minutes == 1 ? "min" : "mins";

                return $"{timeSpan.Minutes} {m} ago";
            }
            else if (timeSpan.TotalHours < 24)
            {
                string h = timeSpan.Hours == 1 ? "hour" : "hours";

                return $"{timeSpan.Hours} {h} ago";
            }
            else if (now.Year == localTime.Year)
            {
                return $"{dateTime.Day:d2}.{dateTime.Month:d2}";
            }
            else
            {
                return $"{dateTime.Day:d2}.{dateTime.Month:d2}.{dateTime.Year}";
            }
        }
    }
}
