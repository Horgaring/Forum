namespace Identityserver.Models.Store;

public interface IImageStore
{
    public void WriteImage(Stream file,string name);
    public Stream ReadImage(string name);
}