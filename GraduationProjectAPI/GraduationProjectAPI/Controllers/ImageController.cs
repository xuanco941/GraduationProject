using GraduationProjectAPI.Models;
using GraduationProjectAPI.Services.Context;
using GraduationProjectAPI.Services.Email;
using GraduationProjectAPI.Services.HistoryImageColorized;
using GraduationProjectAPI.Services.Image;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;

namespace GraduationProjectAPI.Controllers
{

    [ApiController]
    [Route("API/[controller]")]
    public class ImageController
    {


        private readonly IImageService _imageService;
        private readonly IHttpContextMethod _httpContextMethod;
        private readonly IHistoryImageColorizedService _historyImageColorizedService;
        private readonly IEmailService _emailService;




        public ImageController(IImageService imageService, IEmailService emailService, IHttpContextMethod httpContextMethod, IHistoryImageColorizedService historyImageColorizedService)
        {
            _imageService = imageService;
            _httpContextMethod = httpContextMethod;
            _historyImageColorizedService = historyImageColorizedService;
            _emailService = emailService;
        }

        [AllowAnonymous]
        [HttpPost("ColorizeImage")]
        public async Task<IActionResult> Colorize(IFormFile image)
        {
            if (_imageService.IsImageFile(image) == false)
            {
                return new BadRequestResult();
            }
            string fileExtension = Path.GetExtension(image.FileName);
            ImageFormat imageFormat = _imageService.GetImageFormat(fileExtension);
            string contentType = _imageService.GetContentType(fileExtension);
            MemoryStream memStream = new MemoryStream();

            try
            {
                await image.CopyToAsync(memStream);
                Bitmap bm2 = await Task.Run(() => _imageService.Colorize(new Bitmap(memStream)));

                string fileName = Guid.NewGuid() + fileExtension;
                string filePath = Path.Combine("Uploads", fileName);

                FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Delete);

                await Task.Run(() => bm2.Save(fileStream, imageFormat));
                fileStream.Seek(0, SeekOrigin.Begin);

                FileStreamResult fileStreamResult = new FileStreamResult(fileStream, contentType)
                {
                    FileDownloadName = fileName
                };

                //xóa file
                _ = Task.Run(() => File.Delete(filePath));

                return fileStreamResult;
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(e.Message);
            }
        }


        [Authorize]
        [HttpPost("ColorizeImages/{isReceiveEmail}")]
        public async Task<IActionResult> ColorizeImages(List<IFormFile> images, bool isReceiveEmail)
        {
            int userId = _httpContextMethod.GetIDContext();
            List<HistoryImageColorized> results = new List<HistoryImageColorized>();

            foreach (var image in images)
            {

                HistoryImageColorized imageColorization = new HistoryImageColorized();
                imageColorization.UserID = userId;

                if (_imageService.IsImageFile(image) == false)
                {
                    continue;
                }
                try
                {
                    string fileExtension = Path.GetExtension(image.FileName);

                    string contentType = _imageService.GetContentType(fileExtension);

                    ImageFormat imageFormat = _imageService.GetImageFormat(fileExtension);

                    imageColorization.ImageOriginName = Guid.NewGuid() + fileExtension;
                    imageColorization.ImageColorizedName = Guid.NewGuid() + fileExtension;




                    FileStream memStream = new FileStream(Path.Combine("Uploads", imageColorization.ImageOriginName), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read);
                    //luu anh original
                    await image.CopyToAsync(memStream);
                    memStream.Seek(0, SeekOrigin.Begin);
                    //to mau
                    Bitmap bm2 = await Task.Run(() => _imageService.Colorize(new Bitmap(memStream)));
                    //tao file moi
                    FileStream fileStream = new FileStream(Path.Combine("Uploads", imageColorization.ImageColorizedName), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

                    await Task.Run(() => bm2.Save(fileStream, imageFormat));
                    //fileStream.Seek(0, SeekOrigin.Begin);

                    results.Add(imageColorization);


                }
                catch
                {
                    //err
                    continue;
                }
            }

            if (results != null && results.Count > 0)
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        await _historyImageColorizedService.AddRange(results);
                        if (isReceiveEmail == true)
                        {
                            string? email = _httpContextMethod.GetEmailFromContext();
                            if (string.IsNullOrEmpty(email) == true)
                            {
                                await _emailService.SendEmailFromGmail(email, "Tạo màu XWay", $"Đã tô màu xong {results.Count} ảnh của bạn, bạn đã có thể truy cập trang web để xem");

                            }
                        }
                    }
                    catch
                    {

                    }
                });


                return new OkObjectResult(results);
            }
            else
            {
                return new BadRequestResult();
            }


        }





        //[Authorize]
        //[HttpPost("ColorizeImages")]
        //public async Task<IActionResult> ColorizeImages(List<IFormFile> images)
        //{
        //    List<HistoryImageColorized> results = new List<HistoryImageColorized>();

        //    foreach (var image in images)
        //    {

        //        HistoryImageColorized imageColorization = new HistoryImageColorized();

        //        if (_imageService.IsImageFile(image) == false)
        //        {
        //            continue;
        //        }
        //        try
        //        {
        //            string fileExtension = Path.GetExtension(image.FileName);

        //            string contentType = _imageService.GetContentType(fileExtension);

        //            ImageFormat imageFormat = _imageService.GetImageFormat(fileExtension);

        //            imageColorization.ImageOriginName = Guid.NewGuid() + fileExtension;
        //            imageColorization.ImageColorizedName = Guid.NewGuid() + fileExtension;




        //            FileStream memStream = new FileStream(Path.Combine("Uploads", imageColorization.ImageOriginName), FileMode.CreateNew, FileAccess.ReadWrite, FileShare.Read);
        //            //luu anh original
        //            await image.CopyToAsync(memStream);
        //            memStream.Seek(0, SeekOrigin.Begin);
        //            //to mau
        //            Bitmap bm2 = await Task.Run(() => _imageService.Colorize(new Bitmap(memStream)));
        //            //tao file moi
        //            FileStream fileStream = new FileStream(Path.Combine("Uploads", imageColorization.ImageColorizedName), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);

        //            await Task.Run(() => bm2.Save(fileStream, imageFormat));
        //            //fileStream.Seek(0, SeekOrigin.Begin);

        //            results.Add(imageColorization);
        //        }
        //        catch
        //        {
        //            //err
        //            continue;
        //        }
        //    }

        //    if (results != null && results.Count > 0)
        //    {
        //        return new OkObjectResult(results);
        //    }
        //    else
        //    {
        //        return new BadRequestResult();
        //    }


        //}



        [Authorize]
        [HttpGet("{imageUrl}")]
        public async Task<IActionResult> GetImage(string imageUrl)
        {
            try
            {
                string extension = Path.GetExtension(imageUrl);
                string contentType = _imageService.GetContentType(extension);
                FileStream fileStream = new FileStream(Path.Combine("Uploads", imageUrl), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                return new FileStreamResult(fileStream, contentType)
                {
                    FileDownloadName = imageUrl
                };
            }
            catch
            {
                return new NotFoundResult();
            }

        }

    }
}
