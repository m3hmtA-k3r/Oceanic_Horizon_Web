using AutoMapper;
using Oceanic_Horizon_Travel.DTOs.BookingDtos;
using Oceanic_Horizon_Travel.Entities;

namespace Oceanic_Horizon_Travel.Mappings
{
    public class BookingMappings: Profile
    {
        public BookingMappings()
        {
            CreateMap<Booking, ResultBookingDto>();
            CreateMap<Booking, DetailBookingDto>();
        }
    }
}
