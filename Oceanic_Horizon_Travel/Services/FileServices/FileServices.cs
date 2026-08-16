
namespace Oceanic_Horizon_Travel.Services.FileServices
{
    public class FileServices : IFileServices
    {
        private readonly IWebHostEnvironment _env;
        public FileServices(IWebHostEnvironment env)
        {
            _env = env;
        }


        public async Task<string> SaveAsync(IFormFile file, string folderName)
        {
            var folder = Path.Combine(_env.WebRootPath, "uploads", folderName);

            if(!Directory.Exists(folder)) 
                Directory.CreateDirectory(folder);


            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{folderName}/{fileName}";
        }
    }
}
