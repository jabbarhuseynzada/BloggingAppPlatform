using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Core.Helpers.Business;

public class AddPhotoHelper(IWebHostEnvironment webHostEnvironment) : IAddPhotoHelperService
{
    private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
    public void AddImage(IFormFile formFile, string guid)
    {
        var fileName = guid;
        var wwwFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
        var imageFolder = Path.Combine(wwwFolder, fileName);
        using var fileStream = new FileStream(imageFolder, FileMode.Create);
        formFile.CopyTo(fileStream);
    }
}