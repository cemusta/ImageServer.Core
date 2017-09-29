namespace ImageServer.Core.Services
{
    public interface IImageService
    {
        byte[] GetImageAsBytes(int w, int h, int quality, byte[] bytes, string options);
    }
}

