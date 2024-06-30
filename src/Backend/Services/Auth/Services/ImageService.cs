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
        using var stream = file.OpenReadStream();
        _store.Upload(stream,name);
    }

    public Stream GetImage(string name)
    {
        return _store.DownLoad(name);
    }

    public bool TrySaveImage(IFormFile file, string name)
    {
        try
        {
            SaveImage(file, name);
            return true;
        }
        catch
        {
            return false;
        }
    }
}