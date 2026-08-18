using AutoMapper;
using MongoDB.Driver;
using Oceanic_Horizon_Travel.DTOs.QuestionDtos;
using Oceanic_Horizon_Travel.Entities;
using Oceanic_Horizon_Travel.Services.MemberServices;
using Oceanic_Horizon_Travel.Services.TourServices;
using Oceanic_Horizon_Travel.Settings;

namespace Oceanic_Horizon_Travel.Services.QuestionServices
{
    public class QuestionServices : IQuestionServices
    {
        private readonly IMongoCollection<Question> _questionCollection;
        private readonly ITourServices _tourServices;
        private readonly IMemberServices _memberServices;
        private readonly IMapper _mapper;

        public QuestionServices(IDatabaseSettings databaseSettings, ITourServices tourServices,IMemberServices memberServices, IMapper mapper)
        {
            _tourServices = tourServices;
            _memberServices = memberServices;
            _mapper = mapper;

            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _questionCollection = database.GetCollection<Question>(databaseSettings.QuestionCollectionName);
        }



        public async Task CreateAsync(CreateQuestionDto createQuestionDto)
        {
            var question = new Question
            {
                TourId = createQuestionDto.TourId!,
                MemberId = createQuestionDto.MemberId!,
                Text = createQuestionDto.Text!,
                IsAnswered = false,
                IsApproved = false,
                CreatedDate = DateTime.UtcNow

            };

            await _questionCollection.InsertOneAsync(question);
        }
       
        public async Task<List<ResultQuestionDto>> GetApprovedByTourAsync(string tourId)
        {
            var questions = await _questionCollection.Find(x => x.TourId == tourId && x.IsAnswered && x.IsApproved)
                                                     .SortByDescending(x => x.AnsweredDate)
                                                     .ToListAsync();
            return await EnrichAsync(questions);
        }


        //Admin Kısmı için aşagıdakiler

        public async Task<List<ResultQuestionDto>> GetAllAsync(string? status = null)
        {
            var filter = status switch
            {
                "pending" => Builders<Question>.Filter.Where(x => !x.IsAnswered && !x.IsApproved),
                "answered" => Builders<Question>.Filter.Where(x => x.IsAnswered),
                "published" => Builders<Question>.Filter.Where(x => x.IsApproved),
                "unpublished" => Builders<Question>.Filter.Where(x => x.IsAnswered && !x.IsApproved),
                _ => Builders<Question>.Filter.Empty
            };

            var questions = await _questionCollection.Find(filter).ToListAsync();

            var sort = questions
                .OrderBy(x => x.IsAnswered)
                .ThenBy(x => x.IsApproved)
                .ThenByDescending(x => x.CreatedDate)
                .ToList();

            return await EnrichAsync(sort);

        }
        public async Task<ResultQuestionDto?> GetByIdAsync(string id)
        {
            var question = await _questionCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

            if(question == null) 
                return null;

            var enriched = await EnrichAsync(new List<Question>  { question });
            return enriched.FirstOrDefault();
        }


        public async Task AnswerAsync(AnswerQuestionDto answerQuestionDto, string adminId)
        { // Cevapsız bir soru hiçbir yayına çıkamaz.
            if(answerQuestionDto.IsApproved && string.IsNullOrWhiteSpace(answerQuestionDto.Answer))
            {
                throw new InvalidOperationException("Cevaplanmamış soru yayınlanamaz.");
            }

            var hasAnswer = !string.IsNullOrWhiteSpace(answerQuestionDto.Answer);

            var update = Builders<Question>.Update
                .Set(x => x.Answer, answerQuestionDto.Answer)
                .Set(x => x.IsAnswered, hasAnswer)
                .Set(x => x.IsApproved, answerQuestionDto.IsApproved)
                .Set(x => x.AnsweredDate, hasAnswer ? DateTime.UtcNow : null)
                .Set(x => x.AnsweredByAdminId,hasAnswer ? adminId : null);

            await _questionCollection.UpdateOneAsync(x => x.Id == answerQuestionDto.QuestionId, update);
        }

        public async Task SetApprovalAsync(string id, bool isApproved)
        {
            if (isApproved)
            {
                var question = await _questionCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

                if(question == null || !question.IsAnswered || string.IsNullOrWhiteSpace(question.Answer))
                {
                    throw new InvalidOperationException("Cevaplanmamış soru yayınlanamaz.");
                }
            }

            var update = Builders<Question>.Update.Set(x => x.IsApproved, isApproved);

            await _questionCollection.UpdateOneAsync(x => x.Id == id, update);
        }

        public async Task<int> GetPendingCountAsync()
        {
            var count = await _questionCollection.CountDocumentsAsync(x => !x.IsAnswered && !x.IsApproved);
            return (int)count;// Bu metot admin dashboard'da "Yanıt Bekleyen Sorular" sayacı için kullanılır.
        }
                     
        public async Task DeleteAsync(string id)
        {
            await _questionCollection.DeleteOneAsync(x => x.Id == id);
        }



        // Her soru için ayrı sorgu iletmez kimlikleri toplayıp iki sorguda çeker.
        private async Task<List<ResultQuestionDto>> EnrichAsync(List<Question> questions)
        {
            var result = _mapper.Map<List<ResultQuestionDto>>(questions);
            if(result.Count == 0) 
                return result;

            var tourIds = questions.Select(x => x.TourId).Distinct().ToList();
            var memberIds = questions.Select(x => x.MemberId).Distinct().ToList();

            var tours = await _tourServices.GetByIdsAsync(tourIds);
            var members = await _memberServices.GetByIdsAsync(memberIds);

            var tourMap = tours.ToDictionary(x => x.Id!, x => x.Title.Tr ?? "");
            var memberMap = members.ToDictionary(x => x.Id!, x => $"{x.FirstName} {x.LastName}");



            foreach (var dto in result)
            {
                dto.TourTitle = tourMap.GetValueOrDefault(dto.TourId!, "-");
                dto.MemberName = memberMap.GetValueOrDefault(dto.MemberId!, "-");
            }

            return result;
        }

    }
}
