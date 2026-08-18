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



        public async Task<List<ResultMemberDto>> GetAllAsync()
        {
            var members = await _memberCollection.Find(_ => true).SortByDescending(x => x.CreatedDate).ToListAsync();

            return _mapper.Map<List<ResultMemberDto>>(members);
        }

        public async Task<ResultMemberDto> GetByIdAsync(string id)
        {
            var member = await _memberCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            return _mapper.Map<ResultMemberDto>(member);
        }

        public async Task UpdateRolesAndStatusAsync(UpdateMemberDto updateMemberDto)
        {// Admin sadece rol ve durum değiştirebilir.
         // FindOneAndReplace ile degil — Update.Set ile kısmi güncelleme yapıyoruz,
            var update = Builders<Member>.Update.Set(s => s.Roles, updateMemberDto.Roles)
                                                .Set(z => z.IsActive, updateMemberDto.IsActive);

            await _memberCollection.UpdateOneAsync(x => x.Id == updateMemberDto.Id, update);

        }

        public async Task<List<ResultMemberDto>> GetByIdsAsync(List<string> ids)
        {
           if(ids == null || ids.Count == 0)
                return new List<ResultMemberDto>();

           var members = await _memberCollection.Find(x => ids.Contains(x.Id)).ToListAsync();

            return _mapper.Map<List<ResultMemberDto>>(members);
        }
    }
}
