namespace SocialMedia1.Services.Users
{
    public interface IImageService : IService
    {
        Task<string> UploadImageToCloudinary(IFormFile image, string userId);
    }
}
