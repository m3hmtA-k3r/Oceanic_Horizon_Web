using Oceanic_Horizon_Travel.DTOs.DashboardDtos;

namespace Oceanic_Horizon_Travel.Services.DashboardServices
{
    public interface IDashboardServices
    {
        Task<DashboardViewModel> GetDashboardAsync();
        Task<AdminNotificationDto> GetNotificationsAsync(); //Bildirim zili
    }
}
