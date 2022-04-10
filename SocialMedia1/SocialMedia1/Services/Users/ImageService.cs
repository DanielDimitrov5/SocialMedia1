using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using SocialMedia1.Data;

namespace SocialMedia1.Services.Users
{
    public class ImageService : IImageService
    {
        private readonly Account account;
        private readonly Cloudinary cloudinary;

        public ImageService()
        {
            account = new Account(
            "dani03",
            "714726182434833",
            "BuFfSQmUk6tZXXZk0tM88CZM3nM");

            cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadImageToCloudinary(IFormFile image, string userId)
        {
            if (image == null)
            {
                return null;
            }

            var extension = Path.GetExtension(image.FileName).TrimStart('.');

            if (extension != "jpg" && extension != "png" && extension != "gif")
            {
                return null;
            }

            await UploadImageToServerAsync(image, userId);

            var physicalPath = $@"wwwroot/img/ProfilePictures/{userId}.{extension}";

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(physicalPath),
                PublicId = $"{userId}"
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            return uploadResult.SecureUrl.ToString();
        }

        private async Task<string> UploadImageToServerAsync(IFormFile image, string userId)
        {
            var extension = Path.GetExtension(image.FileName).TrimStart('.');

            var physicalPath = $"wwwroot/img/ProfilePictures/{userId}.{extension}";
            using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
            await image.CopyToAsync(fileStream);

            fileStream.Dispose();

            return extension;
        }
    }
}
