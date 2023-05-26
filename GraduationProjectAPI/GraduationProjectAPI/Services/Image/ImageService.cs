using GraduationProjectAPI.Colorization;
using System.Drawing;
using System.Drawing.Imaging;

namespace GraduationProjectAPI.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly DeOldify _deOldify;

        public ImageService(DeOldify deOldify)
        {
            _deOldify = deOldify;
        }
        public Bitmap Colorize(Bitmap bw)
        {
            return _deOldify.Colorize(bw);
        }
        public bool IsImageFile(IFormFile file)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".ico" };

            string fileExtension = Path.GetExtension(file.FileName);
            string lowerCaseExtension = fileExtension.ToLowerInvariant();

            return imageExtensions.Contains(lowerCaseExtension);
        }

        public string GetContentType(string fileExtension)
        {
            string contentType = "application/octet-stream";

            switch (fileExtension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;

                case ".png":
                    contentType = "image/png";
                    break;

                case ".gif":
                    contentType = "image/gif";
                    break;

                case ".bmp":
                    contentType = "image/bmp";
                    break;

                case ".ico":
                    contentType = "image/x-icon";
                    break;

                default:
                    contentType = "application/octet-stream"; // Kiểu mặc định nếu không xác định được
                    break;
            }

            return contentType;
        }


        public ImageFormat GetImageFormat(string fileExtension)
        {
            ImageFormat imageFormat = ImageFormat.Jpeg;

            switch (fileExtension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    imageFormat = ImageFormat.Jpeg;
                    break;

                case ".png":
                    imageFormat = ImageFormat.Png;
                    break;

                case ".gif":
                    imageFormat = ImageFormat.Gif;
                    break;

                case ".bmp":
                    imageFormat = ImageFormat.Bmp;
                    break;

                case ".ico":
                    imageFormat = ImageFormat.Icon;
                    break;

                default:
                    imageFormat = ImageFormat.Jpeg;
                    break;
            }

            return imageFormat;
        }
    }
}
