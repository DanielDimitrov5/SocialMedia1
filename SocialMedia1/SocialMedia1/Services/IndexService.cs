using SocialMedia1.Models;

namespace SocialMedia1.Services
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
            var postsByFollowedUsers  = postService.GetAllPostsByFollowedUsers(userId);
            var myPosts = postService.GetAllPosts(userId);

            var allPosts = new List<PostViewModel>();

            allPosts.AddRange(postsByFollowedUsers);
            allPosts.AddRange(myPosts);

            return new IndexViewModel
            {
                Posts = allPosts,
            };
        }
    }
}
