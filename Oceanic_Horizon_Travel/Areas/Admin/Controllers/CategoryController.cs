using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Oceanic_Horizon_Travel.DTOs.CategoryDtos;
using Oceanic_Horizon_Travel.Services.CategoryServices;

namespace Oceanic_Horizon_Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController(ICategoryServices _categoryServices, IMapper _mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryServices.GetAllCategoryAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
                return View(createCategoryDto);

            await _categoryServices.CreateAsync(createCategoryDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(string id)
        {
            var category = await _categoryServices.GetByIdAsync(id);
            var updateCategory = _mapper.Map<UpdateCategoryDto>(category);

            return View(updateCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateCategoryDto updateCategoryDto)
        {
            if (!ModelState.IsValid)
                return View(updateCategoryDto);

            await _categoryServices.UpdateAsync(updateCategoryDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _categoryServices.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
