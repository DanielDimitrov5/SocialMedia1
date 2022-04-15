namespace SocialMedia1.Services.Common
{
    public interface INavBarService : IService
    {
        int FollowRequestsCount(string userId);
    }
}
