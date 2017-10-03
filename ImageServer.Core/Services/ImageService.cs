using System.Collections.Generic;
using System.Linq;
using ImageMagick;

namespace ImageServer.Core.Services
{
    public class ImageService: IImageService
    {
        public byte[] GetImageAsBytes(int w, int h, int quality, byte[] bytes, string options)
        {
            MagickImageInfo info = new MagickImageInfo(bytes);

            if (info.Format == MagickFormat.Gif || info.Format == MagickFormat.Gif87)
                return ProcessGif(w, h, quality, bytes, options);

            return ProcessImage(w, h, quality, bytes, options);
        }

        private static byte[] ProcessImage(int w, int h, int quality, byte[] bytes, string options)
        {
            using (MagickImage image = new MagickImage(bytes))
            {
                ResizeSingleImage(w, h, quality, options, image);

                // return the result
                bytes = image.ToByteArray(MagickFormat.Pjpeg);
            }

            return bytes;
        }

        private static byte[] ProcessGif(int w, int h, int quality, byte[] bytes, string options)
        {
            using (MagickImageCollection collection = new MagickImageCollection(bytes))
            {
                collection.Coalesce();

                // the height will be calculated with the aspect ratio.
                foreach (var magickImage in collection)
                {
                    var img = (MagickImage)magickImage;
                    ResizeSingleImage(w, h, quality, options, img);
                    //img.Resize(w, h);
                }

                collection.RePage();

                collection.Optimize();
                collection.OptimizeTransparency();

                // Save the result
                return collection.ToByteArray();
            }
        }

        private static void ResizeSingleImage(int w, int h, int quality, string options, MagickImage image)
        {
            if (options.Contains("f") || options.Contains("t")) //thumbnail
            {
                MagickGeometry size = new MagickGeometry(w, h);
                image.Thumbnail(size);
                image.Quality = quality;
            }
            else
            {
                // This will resize the image to a fixed size without maintaining the aspect ratio.
                MagickGeometry size = new MagickGeometry(w, h)
                {
                    IgnoreAspectRatio = false,
                    FillArea = true
                };
                image.Quality = quality;

                image.Resize(size);
                image.Crop(size);
            }

            if (options.Contains("g")) //grayscale
                image.Grayscale(PixelIntensityMethod.Average);
        }
        
        public string GetVersion()
        {
            return MagickNET.Version;
        }

        public string GetFeatures()
        {
            return MagickNET.Features.Trim();
        }

        public List<MagickFormatInfo> GetSupportedFormats()
        {            
            return MagickNET.SupportedFormats.ToList();
        }
    }
}