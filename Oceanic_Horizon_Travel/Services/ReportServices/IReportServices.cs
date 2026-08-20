using Oceanic_Horizon_Travel.DTOs.ReportDtos;

namespace Oceanic_Horizon_Travel.Services.ReportServices
{
    public interface IReportServices
    {
        Task<ParticipantReportViewModel> GetReportAsync(string? tourDateId);
        Task<byte[]> GenerateExcelAsync(string tourDateId);
        Task<byte[]> GeneratePdfAsync(string tourDateId);
    }
}
