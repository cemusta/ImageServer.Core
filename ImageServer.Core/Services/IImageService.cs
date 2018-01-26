using System.Collections.Generic;
using ImageMagick;
using ImageServer.Core.Model;

namespace ImageServer.Core.Services
{
    public interface IImageService
    {
        byte[] GetImageAsBytes(int requestWidth, int requestHeight, int quality, byte[] bytes, string options, out string mimeType, CustomRatio ratio = null);

        string GetVersion();

        string GetFeatures();

        List<MagickFormatInfo> GetSupportedFormats();
    }
}

