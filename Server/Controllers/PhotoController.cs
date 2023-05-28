using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Schiavello.Shared;
using Server.Core;
using Server.Services;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PhotoController : ControllerBase
{
    private readonly IPhotoService _photoService;
    private readonly DataContext _context;
    public PhotoController(IPhotoService photoService, DataContext context)
    {
        _photoService = photoService;
        _context = context;
    }

    [HttpGet("get-all")]
    public async Task<IEnumerable<PhotoItem>> GetPhotos()
    {
        return await _context.Photos.AsNoTracking()
                            .Select(p => new PhotoItem()
                            {
                                Id = p.Id,
                                Category = p.Category,
                                Title = p.Title,
                                Url = p.Url,
                                Description = p.Description
                            }).ToListAsync();
    }

    [HttpPost("upload-photo")]
    public async Task<PhotoUploadResult> UploadPhoto(IFormFile file)
    {
        var uploadResult = await _photoService.UploadFormFileCloudinary(file);
        return uploadResult;
    }

    [HttpPost("save-photo")]
    public async Task<string> SavePhoto(SavePhotoRequest photo)
    {
        if (string.IsNullOrEmpty(photo.Id))
        {
            var photoToAdd = new Photo();

            photoToAdd.Id = Guid.NewGuid().ToString();
            photoToAdd.Category = photo.Category;
            photoToAdd.Title = photo.Title;
            photoToAdd.Description = photo.Description;
            photoToAdd.Url = photo.Url;

            await _context.Photos.AddAsync(photoToAdd);

            await _context.SaveChangesAsync();

            return photoToAdd.Id;  
        }

        var photoToUpdate = await _context.Photos.FirstOrDefaultAsync(t => t.Id == photo.Id);

        photoToUpdate.Category = photo.Category;
        photoToUpdate.Title = photo.Title;
        photoToUpdate.Description = photo.Description;

        await _context.SaveChangesAsync();

        return photoToUpdate.Id;
    }

    [HttpDelete("delete-photo/{id}")]
    public async Task DeletePhoto(string id)
    {
        var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);

        _context.Photos.Remove(photo);

        await _context.SaveChangesAsync();
    }

}
