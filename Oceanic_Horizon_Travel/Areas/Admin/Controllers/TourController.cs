using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oceanic_Horizon_Travel.DTOs.TourDtos;
using Oceanic_Horizon_Travel.Entities;
using Oceanic_Horizon_Travel.Services.DestinationServices;
using Oceanic_Horizon_Travel.Services.FileServices;
using Oceanic_Horizon_Travel.Services.TourServices;
using System.Threading.Tasks;

[Area("Admin")]
public class TourController(ITourServices _tourServices,IDestinationServices _destinationServices,IFileServices _fileService,
    IMapper _mapper,IWebHostEnvironment _env) : Controller
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
            await LoadDestinationsAsync(createTourDto.DestinationId);
            return View(createTourDto);
        }

        if(createTourDto.ImageFile is not null && createTourDto.ImageFile.Length > 0)
        {
            createTourDto.ThumbnailUrl = await _fileService.SaveAsync(createTourDto.ImageFile, "tours");
        }

        await _tourServices.CreateAsync(createTourDto);
        return RedirectToAction(nameof(Index));

    }

    public async Task<IActionResult> Update(string id)
    {
        var tour = await _tourServices.GetByIDAsync(id);
        var updateTour = _mapper.Map<UpdateTourDto>(tour);

        await LoadDestinationsAsync(updateTour.DestinationId);
        return View(updateTour);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateTourDto updateTourDto)
    {
        if (!ModelState.IsValid)
        {
            await LoadDestinationsAsync(updateTourDto.DestinationId);
            return View(updateTourDto);
        }

        if(updateTourDto.ImageFile is not null && updateTourDto.ImageFile.Length > 0)
        {
            updateTourDto.ThumbnailUrl = await _fileService.SaveAsync(updateTourDto.ImageFile, "tours");
        }

        await _tourServices.UpdateAsync(updateTourDto);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(string id)
    {
        await _tourServices.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // Destinasyon listesini açılır menü için hazırlar
    private async Task LoadDestinationsAsync(string? selectedId = null)
    {
        var destinations = await _destinationServices.GetAllAsync();

        ViewBag.Destinations = new SelectList(
            destinations.Select(x => new
            {
                Id = x.Id,
                Name = $"{x.City.Tr} — {x.Country.Tr}"      // "Kapadokya — Türkiye"
            }),
            "Id",       // option value
            "Name",     // option metni
            selectedId  // hangisi seçili gelsin
        );
    }

    


}
