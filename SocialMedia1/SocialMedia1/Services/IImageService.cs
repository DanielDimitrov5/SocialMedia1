namespace SocialMedia1.Services
{
    public interface IImageService
    {
        Task<string> UploadImageToCloudinary(IFormFile image, string userId);
    }
}
