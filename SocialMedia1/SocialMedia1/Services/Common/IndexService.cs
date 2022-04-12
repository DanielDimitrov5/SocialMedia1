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

            string timeSpan = string.Empty;

            if (now.Day - localTime.Day == 1)
            {
                return "Yesterday";
            }
            else if (now.Day - localTime.Day < 1 && now.Minute - localTime.Minute == 0)
            {
                return "now";
            }
            else if (now.Day - localTime.Day < 1 && now.Minute - localTime.Minute < 60)
            {
                return $"{now.Minute - localTime.Minute} mins ago";
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
