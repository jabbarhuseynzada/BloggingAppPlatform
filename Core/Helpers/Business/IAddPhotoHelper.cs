using Microsoft.AspNetCore.Http;

namespace Core.Helpers.Business;

public interface IAddPhotoHelperService
{
    void AddImage(IFormFile formFile, string guid);
}