using Microsoft.AspNetCore.Mvc;
using Oceanic_Horizon_Travel.Services.ReportServices;

namespace Oceanic_Horizon_Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReportController(IReportServices _reportServices) : Controller
    {
        public async Task<IActionResult> Index(string? tourDateId)
        {
            var model = await _reportServices.GetReportAsync(tourDateId);
            return View(model);
        }

        public async Task<IActionResult> ExportExcel(string tourDateId)
        {
            if (string.IsNullOrWhiteSpace(tourDateId))
                return RedirectToAction(nameof(Index));

            var bytes = await _reportServices.GenerateExcelAsync(tourDateId);
            var fileName = $"katilimci-listesi-{DateTime.Now:yyyyMMdd-HHmm}.xlsx";

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }

        public async Task<IActionResult> ExportPdf(string tourDateId)
        {
            if (string.IsNullOrWhiteSpace(tourDateId))
                return RedirectToAction(nameof(Index));

            var bytes = await _reportServices.GeneratePdfAsync(tourDateId);
            var fileName = $"katilimci-listesi-{DateTime.Now:yyyyMMdd-HHmm}.pdf";

            return File(bytes, "application/pdf", fileName);
        }
    }
}
