using SocialMedia1.Models;

namespace SocialMedia1.Services
{
    public interface IIndexService
    {
        IndexViewModel GetIndexViewAsync(string userId);
    }
}
