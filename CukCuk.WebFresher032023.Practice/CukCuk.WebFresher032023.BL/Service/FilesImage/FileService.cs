using CukCuk.WebFresher032023.Common.ExceptionsError;
using CukCuk.WebFresher032023.Common.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CukCuk.WebFresher032023.BL.Service.FilesImage
{
    public class FileService : IFileService
    {
        private readonly IHostEnvironment _hostEnvironment;

        public FileService(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// - Thực hiện lưu thông tin hình ảnh
        /// </summary>
        /// <param name="imageFile">Thông tin file ảnh</param>
        /// <returns>Trạng thái thêm và tên hình ảnh</returns>
        /// Author: DDKhang (3/7/2023)
        public Tuple<int, string> SaveImage(IFormFile imageFile)
        {
            try
            {
                var contentPath = _hostEnvironment.ContentRootPath;

                List<string> validateErrors = new List<string>();

                // path = "c://projects/productminiapi/uploads" ,not exactly something like that
                var path = Path.Combine(contentPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // Check the allowed extenstions
                var ext = Path.GetExtension(imageFile.FileName);
                var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
                if (!allowedExtensions.Contains(ext))
                {
                    string msg = string.Format("Chỉ các đuôi mở rộng {0} thì được cho phép cho hình ảnh.", string.Join(",", allowedExtensions));
                    //return new Tuple<int, string>(0, msg);
                    validateErrors.Add(msg);
                    throw new ValidateException(validateErrors, ResourceVN.Validate_DevMessage);
                }
                string uniqueString = Guid.NewGuid().ToString();
                // we are trying to create a unique filename here
                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return new Tuple<int, string>(1, newFileName);
            }
            catch (ValidateException ex)
            {
                //return new Tuple<int, string>(0, "Error has occured");
                throw ex;
            }
        }

        /// <summary>
        /// - Thực hiện xóa ảnh
        /// </summary>
        /// <param name="imageFileName">Tên file ảnh</param>
        /// <returns>Trạng thái xóa ảnh</returns>
        /// Author: DDKhang (3/7/2023)
        public bool DeleteImage(string imageFileName)
        {
            try
            {
                var wwwPath = _hostEnvironment.ContentRootPath;
                var path = Path.Combine(wwwPath, "Uploads\\", imageFileName);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new InternalException(ex.Message);
            }

        }
    }
}
