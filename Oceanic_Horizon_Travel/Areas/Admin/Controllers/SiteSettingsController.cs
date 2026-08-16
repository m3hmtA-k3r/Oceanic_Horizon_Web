using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Oceanic_Horizon_Travel.DTOs.SiteSettingsDtos;
using Oceanic_Horizon_Travel.Services.FileServices;
using Oceanic_Horizon_Travel.Services.SiteSettingsServices;

namespace Oceanic_Horizon_Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SiteSettingsController(
        ISiteSettingsServices _siteSettingsServices,
        IFileServices _fileService,
        IMapper _mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var settings = await _siteSettingsServices.GetAsync();

            // Hiç kayıt yoksa boş form göster — ilk kurulumda böyle olur
            var model = settings is null
                ? new UpdateSiteSettingsDto()
                : _mapper.Map<UpdateSiteSettingsDto>(settings);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(UpdateSiteSettingsDto updateSiteSettingsDto)
        {
            if (!ModelState.IsValid)
                return View(updateSiteSettingsDto);

            if (updateSiteSettingsDto.LogoFile is not null && updateSiteSettingsDto.LogoFile.Length > 0)
            {
                updateSiteSettingsDto.LogoUrl = await _fileService.SaveAsync(updateSiteSettingsDto.LogoFile, "settings");
            }

            await _siteSettingsServices.SaveAsync(updateSiteSettingsDto);

            TempData["Success"] = "Site bilgileri kaydedildi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
