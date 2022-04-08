namespace SocialMedia1.Services.Users
{
    public interface IImageService
    {
        Task<string> UploadImageToCloudinary(IFormFile image, string userId);
    }
}
