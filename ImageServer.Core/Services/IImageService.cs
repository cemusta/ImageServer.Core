using System.Collections.Generic;
using ImageMagick;

namespace ImageServer.Core.Services
{
    public interface IImageService
    {
        byte[] GetImageAsBytes(int requestWidth, int requestHeight, int quality, byte[] bytes, string options, out string mimeType);

        string GetVersion();

        string GetFeatures();

        List<MagickFormatInfo> GetSupportedFormats();
    }
}

