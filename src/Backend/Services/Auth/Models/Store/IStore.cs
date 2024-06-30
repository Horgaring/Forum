namespace Identityserver.Models.Store;

public interface IImageStore
{
    public void Upload(Stream file,string name);
    public Stream DownLoad(string name);
}