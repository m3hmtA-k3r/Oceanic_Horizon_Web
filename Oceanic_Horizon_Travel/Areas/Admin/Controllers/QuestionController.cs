using Microsoft.AspNetCore.Mvc;
using Oceanic_Horizon_Travel.DTOs.QuestionDtos;
using Oceanic_Horizon_Travel.Services.QuestionServices;
using System.Security.Claims;

namespace Oceanic_Horizon_Travel.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class QuestionController(IQuestionServices _questionServices) : Controller
    {
        
        public async Task<IActionResult> Index(string? status)
        {// durum: boş - beklemede - yanıtlandı - yayınlandı - yayınlanmadı
            var questions = await _questionServices.GetAllAsync(status);

            ViewBag.CurrentStatus = status ?? "all";
            ViewBag.PendingCount = await _questionServices.GetPendingCountAsync();

            return View(questions);
        }

        public async Task<IActionResult> Answer(string id)
        {
            var question = await _questionServices.GetByIdAsync(id);

            if (question is null) return NotFound();

            ViewBag.Question = question;

            return View(new AnswerQuestionDto
            {
                QuestionId = question.Id,
                Answer = question.Answer,
                IsApproved = question.IsApproved
            });
        }

        // publish: "Kaydet" → false, "Kaydet ve Yayınla" → true
        [HttpPost]
        public async Task<IActionResult> Answer(AnswerQuestionDto answerQuestionDto, bool publish)
        {
            answerQuestionDto.IsApproved = publish;

            var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

            try
            {
                await _questionServices.AnswerAsync(answerQuestionDto, adminId);
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ViewBag.Question = await _questionServices.GetByIdAsync(answerQuestionDto.QuestionId!);
                return View(answerQuestionDto);
            }

            TempData["Success"] = publish ? "Cevap kaydedildi ve yayınlandı." : "Cevap kaydedildi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Publish(string id)
        {
            try
            {
                await _questionServices.SetApprovalAsync(id, true);
                TempData["Success"] = "Soru yayınlandı.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Unpublish(string id)
        {
            await _questionServices.SetApprovalAsync(id, false);

            TempData["Success"] = "Soru yayından kaldırıldı.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            await _questionServices.DeleteAsync(id);

            TempData["Success"] = "Soru silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
