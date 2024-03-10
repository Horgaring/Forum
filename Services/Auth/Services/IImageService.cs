namespace Identityserver.Services;

public interface IImageService
{
    public void SaveImage(IFormFile file,string name);
    public Stream GetImage(string name);
}