using ImageMagick;
using ImageMagick.Formats;
using System;
using System.IO;

namespace SomeBlog.Media
{
    public class Tools
    {
        /// <summary>
        /// https://developers.google.com/speed/docs/insights/OptimizeImages
        /// convert INPUT.jpg -sampling-factor 4:2:0 -strip [-resize WxH] [-quality N] [-interlace JPEG] [-colorspace Gray/sRGB] OUTPUT.jpg
        /// https://stackoverflow.com/questions/7261855/recommendation-for-compressing-jpg-files-with-imagemagick
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string Optimize(string path)
        {
            if (!File.Exists(path))
                return "";
            try
            {
                using (var image = new MagickImage(path))
                {

                    image.Settings.SetDefines(new JpegWriteDefines()
                    {
                        SamplingFactor = JpegSamplingFactor.Ratio420,
                    });

                    image.Strip();
                    image.Quality = 85;
                    image.Interlace = Interlace.Jpeg;
                    image.ColorSpace = ColorSpace.sRGB;

                    image.Write(path);
                }


            }
            catch (Exception exc)
            {
                return "";
            }

            return path;
        }

        public string Crop(string path, string newFilePath, int width, int height)
        {
            if (!File.Exists(path))
                return "";
            try
            {
                using (var image = new MagickImage(path))
                {
                    //talep edilen WxH oranını bul
                    var requestFileRatio = (double)width / (double)height;

                    //mevcut WxG oranına bak
                    var currentFileRatio = (double)image.Width / (double)image.Height;

                    //talep edilen resmin genişliğin yüksekliğe orani, mevcuttan buyuk
                    //yukseklik sabit kalsin, genisligi crop
                    if (requestFileRatio > currentFileRatio)
                    {
                        var newHeight = image.Width * height / width;

                        image.Crop(image.Width, newHeight);
                    }
                    else
                    {
                        var newWidth = image.Height * width / height;
                        image.Crop(newWidth, image.Height);
                    }

                    image.Resize(width, height);
                    image.Write(newFilePath);
                }
            }
            catch (Exception exc)
            {
                return "";
            }

            return newFilePath;
        }

        public string Quality(string path, int quality)
        {
            using (var image = new MagickImage(path))
            {
                image.Strip();
                image.Quality = quality;
                image.Write(path);
            }

            return path;
        }

        public string Watermark(string path, int position, string color, string text)
        {
            int x = 0;
            int y = 0;

            using (var image = new MagickImage(path))
            {
                using (var copyright = new MagickImage("xc:none", image.Width, image.Height))
                {
                    switch (position)
                    {
                        case 1: x = 10; y = 10; break;
                        case 2: x = 10; y = image.Height - 40; break;
                        case 3: x = image.Width - 100; y = 10; break;
                        case 4: x = image.Width - 100; y = image.Height - 40; break;
                        default: x = 10; y = 10; break;
                    }

                    image.Draw(new Drawables()
                        .FillColor(new MagickColor(color))
                        .Gravity(Gravity.Northwest)
                        .Rotation(0)
                        .Text(x, y, text));


                    image.Tile(copyright, CompositeOperator.Over);
                    image.Write(path);
                }
            }

            return path;
        }

        public string Resize(string path, int width, int height)
        {
            using (var image = new MagickImage(path))
            {
                image.Resize(width, height);
                image.Write(path);
            }

            return path;
        }
    }
}
