using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Oceanic_Horizon_Travel.DTOs.BannerDtos;
using Oceanic_Horizon_Travel.Services.BannerServices;

namespace Oceanic_Horizon_Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BannerController(IBannerServices _bannerServices,IMapper _mapper, IWebHostEnvironment _env): Controller
    {
        public async Task<IActionResult> Index()
        {
            var banners = await _bannerServices.GetAllAsync();
            return View(banners);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBannerDto createBannerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createBannerDto);
            }

            if (createBannerDto.ImageFile is not null && createBannerDto.ImageFile.Length > 0)
            {
                createBannerDto.ImageUrl = await SaveImageAsync(createBannerDto.ImageFile);
            }

            await _bannerServices.CreateAsync(createBannerDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(string id)
        {
            var banner = await _bannerServices.GetByIdAsync(id);
            var updateBanner = _mapper.Map<UpdateBannerDto>(banner);

            return View(updateBanner);

        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateBannerDto updateBannerDto)
        {

            if (!ModelState.IsValid)
            {
                return View(updateBannerDto);
            }

            if (updateBannerDto.ImageFile is not null && updateBannerDto.ImageFile.Length > 0)
            {
                updateBannerDto.ImageUrl = await SaveImageAsync(updateBannerDto.ImageFile);
            }


            await _bannerServices.UpdateAsync(updateBannerDto);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(string id)
        {
            await _bannerServices.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // Yüklenen dosyayı wwwroot/uploads/banners altına kaydeder, web yolunu döner
        private async Task<string> SaveImageAsync(IFormFile file)
        {
            var folder = Path.Combine(_env.WebRootPath, "uploads", "banners");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // Aynı adlı dosyalar birbirini ezmesin diye benzersiz ad üretiyoruz
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/banners/{fileName}";
        }


    }


}
