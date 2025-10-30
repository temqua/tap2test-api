using Microsoft.AspNetCore.Mvc;

namespace Tap2Test_Api.Controllers
{
    [Route("api/videos")]
    [ApiController]
    public class VideosController : ControllerBase
    {
        private readonly string _videoPath = Path.Combine(Directory.GetCurrentDirectory(), "Videos");

        public VideosController()
        {
            if (!Directory.Exists(_videoPath))
                Directory.CreateDirectory(_videoPath);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var files = Directory.GetFiles(_videoPath)
                     .Select(Path.GetFileName)
                     .ToArray();
            return Ok(files);
        }

        [HttpGet("{fileName}")]
        public IActionResult GetVideo(string fileName)
        {
            var filePath = Path.Combine(_videoPath, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();


            var contentType = "video/" + Path.GetExtension(filePath).Trim('.');
            return PhysicalFile(filePath, contentType);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "File not provided" });


            var isVideo = file.ContentType.Contains("video/");
            if (!isVideo)
            {
                return BadRequest(new { message = "You must upload video" });
            }
            var filePath = Path.Combine(_videoPath, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            return Ok(new { file.FileName });
        }


        [HttpPut("{fileName}")]
        public async Task<IActionResult> Update(string fileName, IFormFile file)
        {
            var filePath = Path.Combine(_videoPath, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var isVideo = file.ContentType.Contains("video/");
            if (!isVideo)
            {
                return BadRequest(new { message = "You must upload video" });
            }
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(new { fileName });
        }

        [HttpDelete("{fileName}")]
        public IActionResult Delete(string fileName)
        {
            var filePath = Path.Combine(_videoPath, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();


            System.IO.File.Delete(filePath);
            return NoContent();
        }
    }
}
