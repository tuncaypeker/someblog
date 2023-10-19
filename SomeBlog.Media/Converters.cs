using ImageMagick;
using ImageMagick.Formats;
using SomeBlog.Media.Dto;
using System;
using System.IO;

namespace SomeBlog.Media
{
    public class Converters
    {
        public ConvertResponseDto ToWebP(string path)
        {
            var convertResponse = new ConvertResponseDto();

            if (!File.Exists(path))
            {
                convertResponse.IsSucceed = false;
                convertResponse.Message = "Path not found";
                convertResponse.Path = "";

                return convertResponse;
            }

            var directory = Path.GetDirectoryName(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var requestedFilePath = $"{directory}\\{fileName}.webp";
            if (string.IsNullOrEmpty(directory))
                requestedFilePath = $"{fileName}.webp";

            convertResponse.Path = requestedFilePath;

            if (File.Exists(requestedFilePath))
            {
                convertResponse.IsSucceed = true;
                convertResponse.Message = "already exists";

                return convertResponse;
            }

            try
            {
                using (var images = new MagickImageCollection(path))
                {
                    var defines = new WebPWriteDefines
                    {
                        Lossless = true,
                        Method = 6,
                    };

                    images.Write(requestedFilePath, defines);

                    convertResponse.IsSucceed = true;
                    convertResponse.Message = "converted";

                    return convertResponse;
                }
            }
            catch (Exception exc)
            {
                convertResponse.IsSucceed = false;
                convertResponse.Message = exc.Message;
                convertResponse.Path = "";

                return convertResponse;
            }
        }

        public ConvertResponseDto ToAvif(string path)
        {
            var convertResponse = new ConvertResponseDto();

            if (!File.Exists(path))
            {
                convertResponse.IsSucceed = false;
                convertResponse.Message = "Path not found";
                convertResponse.Path = "";

                return convertResponse;
            }

            var directory = Path.GetDirectoryName(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            var requestedFilePath = $"{directory}\\{fileName}.avif";
            if (string.IsNullOrEmpty(directory))
                requestedFilePath = $"{fileName}.avif";

            convertResponse.Path = requestedFilePath;

            if (File.Exists(requestedFilePath))
            {
                convertResponse.IsSucceed = true;
                convertResponse.Message = "already exists";

                return convertResponse;
            }

            try
            {
                using (var images = new MagickImage(path))
                {
                    images.Write(requestedFilePath, MagickFormat.Avif);

                    convertResponse.IsSucceed = true;
                    convertResponse.Message = "converted";

                    return convertResponse;
                }
            }
            catch (Exception exc)
            {
                convertResponse.IsSucceed = false;
                convertResponse.Message = exc.Message;
                convertResponse.Path = "";

                return convertResponse;
            }
        }
    }
}
