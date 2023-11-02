using Microsoft.EntityFrameworkCore.Migrations;

namespace Identityserver.Models.Store;

public class FileSystemStore : IImageStore
{
    private readonly IConfiguration _config;
    private readonly string _path;

    public FileSystemStore(IConfiguration config)
    {
        _config = config;
        _path = Path.GetDirectoryName(_config["ImagesFolder"] ?? throw new ArgumentNullException());
    }

    public void WriteImage(Stream file,string name)
    {
        if (!Directory.Exists(_path))
        {
            Directory.CreateDirectory(_path);
        }
        using var stream = new FileStream(Path.Combine(_path, name + ".jpeg"), FileMode.OpenOrCreate);
        file.CopyTo(stream);
    }

    public Stream ReadImage(string name)
    {
        return new FileStream(Path.Combine(_path, name + ".jpeg"),FileMode.Open);
    }
}