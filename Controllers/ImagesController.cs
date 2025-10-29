using Microsoft.AspNetCore.Mvc;

namespace Tap2Test_Api.Controllers
{
    [Route("api/images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ILogger<ImagesController> _logger;

        private readonly string _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Images");

        public ImagesController(ILogger<ImagesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var files = Directory.GetFiles(_imagePath)
                     .Select(Path.GetFileName)
                     .ToArray();
            return Ok(files);
        }

        [HttpGet("{fileName}")]
        public IActionResult Get(string fileName)
        {
            var filePath = Path.Combine(_imagePath, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();


            var contentType = "image/" + Path.GetExtension(filePath).Trim('.');
            return PhysicalFile(filePath, contentType);
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not provided");


            var filePath = Path.Combine(_imagePath, file.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            return Ok(new { file.FileName });
        }

        [HttpPut("{fileName}")]
        public async Task<IActionResult> UpdateImage(string fileName, IFormFile file)
        {
            var filePath = Path.Combine(_imagePath, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            return Ok(new { fileName });
        }


        [HttpDelete("{fileName}")]
        public IActionResult DeleteImage(string fileName)
        {
            var filePath = Path.Combine(_imagePath, fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();


            System.IO.File.Delete(filePath);
            return NoContent();
        }
    }
}
