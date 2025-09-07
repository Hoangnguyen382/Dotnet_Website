using System.Collections.Immutable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using DoAn_WebAPI.Models;
using SixLabors.ImageSharp;

namespace DoAn_WebAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly FileUploadSettings _settings;
        private readonly IWebHostEnvironment _env;
        public UploadController(IOptions<FileUploadSettings> settings, IWebHostEnvironment env)
        {
            _settings = settings.Value;
            _env = env;
        }

        //API
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                // Kiểm tra xem file có null hay không
                if (file == null || file.Length == 0)
                {
                    return BadRequest("File không hợp lệ");
                }
                //kiểm tra kich thước file có vuợt quá kích thước tối đa không
                if (file.Length > _settings.MaxFileSize)
                {
                    return BadRequest("File quá lớn");
                }
                // kiểm tra định dạng file
                //B1: lấy đuôi file
                var extension = Path.GetExtension(file.FileName)?.ToLowerInvariant();
                //B2: kiểm tra đuôi file có trong danh sách cho phép không
                if (!_settings.AllowedExtensions.Contains(extension))
                {
                    return BadRequest("File không hợp lệ");
                }
                // B3: tạo tên file trước khi lưu 
                var fileName = $"{Guid.NewGuid()}{extension}";
                var uploadPath = Path.Combine(_env.ContentRootPath, _settings.UploadPath);
                // Lưu file
                if (!Directory.Exists(uploadPath))
                {
                    // nếu folder chưa tồn tại thì tạo mới
                    Directory.CreateDirectory(uploadPath);
                }
                var image = await Image.LoadAsync(file.OpenReadStream());
                // lưu file
                var filePath = Path.Combine(uploadPath, fileName);
                await image.SaveAsync(filePath);
                return Ok(new UploadResponse
                {
                    Success = true,
                    Message = "Upload thành công",
                    FilePath = $"/uploads/{fileName}"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}