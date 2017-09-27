using ImageMagick;

namespace ImageServer.Core.Helpers
{
    public static class ImageHelper
    {
        public static byte[] GetImageAsBytes(int w, int h, int quality, byte[] bytes, string options)
        {
            // Get image from file
            using (MagickImage image = new MagickImage(bytes))
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

                if(options.Contains("g")) //grayscale
                    image.Grayscale(PixelIntensityMethod.Average);

                // Save the result
                bytes = image.ToByteArray();
            }

            return bytes;
        }
    }
}
