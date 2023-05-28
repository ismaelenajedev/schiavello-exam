using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Schiavello.Shared;
using Server.Extensions;

namespace Server.Services;

public interface IPhotoService
{
    Task<PhotoUploadResult> UploadFormFileCloudinary(IFormFile file, string path = "");
}

public class PhotoService : IPhotoService
{
    private readonly string _slash = Path.DirectorySeparatorChar.ToString();
    private readonly Cloudinary _cloudinary;

    public PhotoService(IConfiguration configuration, IOptions<CloudinarySettings> cloudinaryConfig)
    {
        var account = new Account(
            cloudinaryConfig.Value.CloudName,
            cloudinaryConfig.Value.ApiKey,
            cloudinaryConfig.Value.ApiSecret
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task<PhotoUploadResult> UploadFormFileCloudinary(IFormFile file, string path = "")
    {
        var fileName = GetFileName(file.FileName);

        var response = new PhotoUploadResult();

        if (InvalidFileName(fileName))
        {
            response.Success = false;
            response.Failures = "Failure! Invalid Filename ..";

            return response;
        }

        try
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(fileName, stream),
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            response.Success = true;
            response.PublicId = uploadResult.PublicId;
            response.SecureUrl = uploadResult.SecureUrl.ToString();
            response.Url = uploadResult.Url.ToString();

            return response;
        }
        catch (Exception ex)
        {
            return new PhotoUploadResult()
            {
                Success = false,
                Failures = "Failure! An error has occurred .."
            };
        }
    }

    bool InvalidFileName(string fileName)
    {
        List<string> fileExtensions = new List<string>() { "png", "gif", "jpeg", "jpg", "zip", "7z", "pdf", "doc", "docx", "xls", "xlsx", "mp3", "mp4", "avi" };
        string configFileExtensions = "png,jpeg,jpg";

        if (!string.IsNullOrEmpty(configFileExtensions))
        {
            fileExtensions = new List<string>(configFileExtensions.Split(','));
        }

        foreach (string ext in fileExtensions)
        {
            if (fileName.EndsWith(ext))
            {
                return false;
            }
        }

        return true;
    }

    string GetFileName(string fileName)
    {
        if (fileName.Contains(_slash))
        {
            fileName = fileName.Substring(fileName.LastIndexOf(_slash));
            fileName = fileName.Replace(_slash, "");
        }

        if (fileName.StartsWith("mceclip0"))
        {
            Random rnd = new Random();
            fileName = fileName.Replace("mceclip0", rnd.Next(100000, 999999).ToString());
        }
        return fileName.SanitizePath();
    }

}

public class CloudinarySettings
{
    public string CloudName { get; set; }
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
}
