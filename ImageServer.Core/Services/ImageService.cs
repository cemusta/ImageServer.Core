using System;
using System.Collections.Generic;
using System.Linq;
using ImageMagick;
using ImageServer.Core.Model;

namespace ImageServer.Core.Services
{
    public class ImageService : IImageService
    {
        public byte[] GetImageAsBytes(int w, int h, int quality, byte[] bytes, string options, out string mimeType, CustomRatio ratio = null)
        {
            MagickImageInfo info = new MagickImageInfo(bytes);

            if (info.Format == MagickFormat.Gif || info.Format == MagickFormat.Gif87)
            {
                mimeType = "image/gif";
                return ProcessGif(info, w, h, quality, bytes, options);
            }

            return ProcessImage(info, w, h, quality, bytes, options, ratio, out mimeType);
        }

        private static byte[] ProcessImage(MagickImageInfo info, int w, int h, int quality, byte[] bytes, string options, CustomRatio ratio, out string mimeType)
        {
            using (MagickImage image = new MagickImage(bytes))
            {
                if (ratio != null)
                {
                    CropSingleImage(info, w, h, quality, options, image, ratio);
                }
                else
                {
                    ResizeSingleImage(info, w, h, quality, options, image);
                }

                // return the result
                if (image.HasAlpha)
                {
                    mimeType = "image/png";
                    bytes = image.ToByteArray(MagickFormat.Png);
                }
                else
                {
                    mimeType = "image/jpeg";
                    bytes = image.ToByteArray(MagickFormat.Pjpeg);
                }
            }
            return bytes;
        }

        private static byte[] ProcessGif(MagickImageInfo info, int w, int h, int quality, byte[] bytes, string options)
        {
            using (MagickImageCollection collection = new MagickImageCollection(bytes))
            {
                collection.Coalesce();

                // the height will be calculated with the aspect ratio.
                foreach (var magickImage in collection)
                {
                    var img = (MagickImage)magickImage;
                    ResizeSingleImage(info, w, h, quality, options, img);
                    //img.Resize(w, h);
                }

                collection.RePage();

                collection.Optimize();
                collection.OptimizeTransparency();

                // Save the result
                return collection.ToByteArray();
            }
        }

        private static void CropSingleImage(MagickImageInfo info, int w, int h, int quality, string options, MagickImage image, CustomRatio ratio)
        {
            int ratioW = Math.Abs(ratio.X2 - ratio.X1);
            int ratioH = Math.Abs(ratio.Y2 - ratio.Y1);

            if (info.Width == ratioW && info.Height == ratioH) //requested image is same size
            {
                return;
            }

            MagickGeometry cropSize = new MagickGeometry(ratioW, ratioH)
            {
                IgnoreAspectRatio = false,
                FillArea = false,
                X = ratio.X1,
                Y = ratio.Y1
            };

            MagickGeometry size = new MagickGeometry(w, h)
            {
                IgnoreAspectRatio = false,
                FillArea = false
            };

            image.Crop(cropSize);
            image.Resize(size);

            image.Quality = quality;

            if (options.Contains("g")) //grayscale
                image.Grayscale(PixelIntensityMethod.Average);
        }

        private static void ResizeSingleImage(MagickImageInfo info, int w, int h, int quality, string options, MagickImage image)
        {
            if (info.Width == w && info.Height == h) //requested image is same size
            {
                return;
            }
            if (w == 0 && h == 0) //requested image is same size
            {
                return;
            }

            if (options.Contains("f") || options.Contains("t")) //scale with aspect of image
            {
                MagickGeometry size = new MagickGeometry(w, h);
                image.Thumbnail(size);
            }
            else if (w == 0 || h == 0) //scale with aspect of image
            {
                MagickGeometry size = new MagickGeometry(w, h);
                image.Thumbnail(size);
            }
            else // This will resize the image to a fixed size without maintaining the aspect ratio.
            {
                MagickGeometry size = new MagickGeometry(w, h)
                {
                    IgnoreAspectRatio = false,
                    FillArea = true,

                };
                image.Resize(size);
                image.Crop(size, Gravity.Center);
            }

            image.Quality = quality;

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