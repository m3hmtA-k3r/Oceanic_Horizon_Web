using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Oceanic_Horizon_Travel.DTOs.DestinationDtos;
using Oceanic_Horizon_Travel.Entities;
using Oceanic_Horizon_Travel.Services.DestinationServices;
using System.Threading.Tasks;

namespace Oceanic_Horizon_Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DestinationController(IMapper _mapper,IDestinationServices _destinationServices, IWebHostEnvironment _env) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var destinations = await _destinationServices.GetAllAsync();
            return View(destinations);
        }
    

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDestinationDto createDestinationDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createDestinationDto);
            }

            if(createDestinationDto.ImageFile is not null && createDestinationDto.ImageFile.Length > 0)
            {
                createDestinationDto.ThumbnailUrl = await SaveImageAsync(createDestinationDto.ImageFile);
            }

            await _destinationServices.CreateAsync(createDestinationDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(string id)
        {
            var destination = await _destinationServices.GetByIdAsync(id);
            var updateDestination = _mapper.Map<UpdateDestinationDto>(destination);

            return View(updateDestination);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UpdateDestinationDto updateDestinationDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateDestinationDto);
            }

            if (updateDestinationDto.ImageFile is not null && updateDestinationDto.ImageFile.Length > 0)
            {
                updateDestinationDto.ThumbnailUrl = await SaveImageAsync(updateDestinationDto.ImageFile);
            }

            await _destinationServices.UpdateAsync(updateDestinationDto);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(string id)
        {
            await _destinationServices.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            var folder = Path.Combine(_env.WebRootPath, "uploads", "destinations");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            // Aynı adlı dosyalar birbirini ezmesin diye benzersiz ad üretiyoruz
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(folder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/destinations/{fileName}";
        }
        
    }
}
