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
            return new IndexViewModel
            {
                Posts = postService.GetAllPostsByFollowedUsers(userId),
            };
        }
    }
}
