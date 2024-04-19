using Identityserver.Models.Store;

namespace Identityserver.Services;

public class ImageService : IImageService
{
    private readonly IImageStore _store;

    public ImageService(IImageStore store)
    {
        _store = store;
    }

    public void SaveImage(IFormFile file,string name)
    {
        _store.WriteImage(file.OpenReadStream(),name);
    }

    public Stream GetImage(string name)
    {
        return _store.ReadImage(name);
    }
}