
using Oceanic_Horizon_Travel.DTOs.MemberDtos;
using Oceanic_Horizon_Travel.Entities;


namespace Oceanic_Horizon_Travel.Services.MemberServices
{
    public interface IMemberServices
    {
        Task<bool> IsEmailExistAsync(string email);
        Task RegisterAsync(RegisterMemberDto registerMemberDto);
        Task<Member> LoginAsync(LoginMemberDto loginMemberDto);
    }
}
