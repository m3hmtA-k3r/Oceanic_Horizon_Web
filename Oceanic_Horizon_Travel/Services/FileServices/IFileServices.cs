namespace Oceanic_Horizon_Travel.Services.FileServices
{
    public interface IFileServices
    {
        // Dosyayı wwwroot/uploads/{folderName} altına kaydeder, web yolunu döner
        Task<string> SaveAsync(IFormFile file, string folderName);
    }
}
