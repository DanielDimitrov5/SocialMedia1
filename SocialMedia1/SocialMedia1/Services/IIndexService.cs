using SocialMedia1.Models;

namespace SocialMedia1.Services
{
    public interface IIndexService
    {
        IndexViewModel GetIndexView(string userId);
    }
}
