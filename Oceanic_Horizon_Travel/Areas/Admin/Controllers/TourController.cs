using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oceanic_Horizon_Travel.DTOs.TourDtos;
using Oceanic_Horizon_Travel.Entities;
using Oceanic_Horizon_Travel.Entities.SubDocuments;
using Oceanic_Horizon_Travel.Services.CategoryServices;
using Oceanic_Horizon_Travel.Services.DestinationServices;
using Oceanic_Horizon_Travel.Services.FileServices;
using Oceanic_Horizon_Travel.Services.TourServices;
using System.Threading.Tasks;
namespace Oceanic_Horizon_Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TourController(ITourServices _tourServices, IDestinationServices _destinationServices, IFileServices _fileService,
    IMapper _mapper, ICategoryServices _categoryServices) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var tours = await _tourServices.GetAllAsync();
            return View(tours);
        }

        public async Task<IActionResult> Create()
        {
            await LoadDestinationsAsync();// Listeyi doldur
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTourDto createTourDto)
        {
            if (!ModelState.IsValid)
            {
                await LoadDestinationsAsync(createTourDto.DestinationId, createTourDto.CategoryId);
                return View(createTourDto);
            }

            if (createTourDto.ImageFile is not null && createTourDto.ImageFile.Length > 0)
            {
                createTourDto.ThumbnailUrl = await _fileService.SaveAsync(createTourDto.ImageFile, "tours");
            }

            AssignTourDateIds(createTourDto.TourDates);
            await SaveGalleryAsync(createTourDto.GalleryFiles, createTourDto.Images);


            await _tourServices.CreateAsync(createTourDto);
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(string id)
        {
            var tour = await _tourServices.GetByIDAsync(id);
            var updateTour = _mapper.Map<UpdateTourDto>(tour);

            await LoadDestinationsAsync(updateTour.DestinationId, updateTour.CategoryId);
            return View(updateTour);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateTourDto updateTourDto)
        {
            if (!ModelState.IsValid)
            {
                await LoadDestinationsAsync(updateTourDto.DestinationId, updateTourDto.CategoryId);
                return View(updateTourDto);
            }

            if (updateTourDto.ImageFile is not null && updateTourDto.ImageFile.Length > 0)
            {
                updateTourDto.ThumbnailUrl = await _fileService.SaveAsync(updateTourDto.ImageFile, "tours");
            }

            AssignTourDateIds(updateTourDto.TourDates);
            await SaveGalleryAsync(updateTourDto.GalleryFiles, updateTourDto.Images);


            await _tourServices.UpdateAsync(updateTourDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _tourServices.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


        private async Task LoadDestinationsAsync(string? destinationId = null, string? categoryId = null)
        {// Destinasyon listesini açılır menü için hazırlar
            var destinations = await _destinationServices.GetAllAsync();
            var categories = await _categoryServices.GetAllCategoryAsync();

            ViewBag.Destinations = new SelectList(
                destinations.Select(x => new
                {
                    Id = x.Id,
                    Name = $"{x.City.Tr} — {x.Country.Tr}"
                }), "Id", "Name", destinationId);

            ViewBag.Categories = new SelectList(
                categories.Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name.Tr,
                }), "Id", "Name", categoryId);

        }


        private static void AssignTourDateIds(List<TourDate> tourDates)
        {// Booking.TourDateId bu kimliğe bağlanacağı için burada üretiyoruz.
            if (tourDates is null) return;

            foreach (var date in tourDates)
            {
                if (string.IsNullOrEmpty(date.Id))
                    date.Id = Guid.NewGuid().ToString();
                if (date.AvailableSeats == 0)
                    date.AvailableSeats = date.Quota;
            }
        }

        // Galeri dosyalarını kaydeder ve Images listesine ekler
        private async Task SaveGalleryAsync(List<IFormFile>? files, List<ImageItem> images)
        {
            if (files is null || files.Count == 0) return;

            var order = images.Count;

            foreach (var file in files)
            {
                if (file.Length == 0) continue;

                var url = await _fileService.SaveAsync(file, "tours");

                images.Add(new ImageItem
                {
                    Url = url,
                    Order = order++,
                    IsCover = false
                });
            }
        }


    }

}
