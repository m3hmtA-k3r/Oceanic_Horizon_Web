using AutoMapper;
using MongoDB.Driver;
using Oceanic_Horizon_Travel.DTOs.MemberDtos;
using Oceanic_Horizon_Travel.Entities;
using Oceanic_Horizon_Travel.Settings;

namespace Oceanic_Horizon_Travel.Services.MemberServices
{ 
    public class MemberServices : IMemberServices
    {
        private readonly IMongoCollection<Member> _memberCollection;
        private readonly IMapper _mapper;

        public MemberServices(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            _mapper = mapper;
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _memberCollection = database.GetCollection<Member>(databaseSettings.MemberCollectionName);
        }

        public async Task<bool> IsEmailExistAsync(string email)
        { // Bu e-posta ile kayıtlı üye var mı  
            var member = await _memberCollection.Find(x => x.Email == email).FirstOrDefaultAsync();
            return member != null;
        }
        public async Task RegisterAsync(RegisterMemberDto registerMemberDto)
        {
            var member = _mapper.Map<Member>(registerMemberDto);

            // Parola direk olarak kaydetmiyoruz bunu BCrypt ile hashlayıip gizliyoruz
            member.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerMemberDto.Password);
            member.CreatedDate = DateTime.UtcNow;
            member.IsActive = true;
            member.Roles = new List<string> { "Member" };

            await _memberCollection.InsertOneAsync(member); 



        }

        public async Task<Member> LoginAsync(LoginMemberDto loginMemberDto)
        {
            var member = await _memberCollection.Find(x => x.Email == loginMemberDto.Email).FirstOrDefaultAsync();

            if (member == null)
                return null;

            var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(loginMemberDto.Password, member.PasswordHash);

            if (!isPasswordCorrect)
                return null;

            return member;


        }
    }
}
