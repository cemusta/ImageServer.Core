using System.Collections.Generic;
using ImageMagick;

namespace ImageServer.Core.Services
{
    public interface IImageService
    {
        byte[] GetImageAsBytes(int w, int h, int quality, byte[] bytes, string options);

        string GetVersion();

        string GetFeatures();

        List<MagickFormatInfo> GetSupportedFormats();
    }
}

