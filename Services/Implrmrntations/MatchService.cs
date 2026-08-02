using LostAndFoundAPI.DTOs;
using LostAndFoundAPI.Repositories.Interfaces;
using LostAndFoundAPI.Services.Interfaces;


namespace LostAndFoundAPI.Services.Implementations
{
    public class  MatchService : IMatchService
    {
        private readonly ILostItemRepository _lostRepository;
        private readonly IFoundItemRepository _foundRepository;

        public MatchService(ILostItemRepository lostRepository, IFoundItemRepository foundRepository)
        {
            _lostRepository = lostRepository;
            _foundRepository = foundRepository;
        }


        private int CalculateWordScore(string text1, string text2, int maxScore)
        {
            var Words1 = text1.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var Words2 = text2.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (Words1.Length == 0 || Words2.Length == 0)
            {
                return 0;
            }
            var commonWords = Words1.Intersect(Words2);
            var matchingWords = commonWords.Count();
            return (int)Math.Round(((double)matchingWords / Words1.Length) * maxScore);
        }


        private int calculateDateScore(DateTime lostDate, DateTime foundDate)
        {
            var daysDifference= Math.Abs((lostDate - foundDate).Days);
            if (daysDifference ==0)
            {
                return 10;
            }
            else if (daysDifference ==1)
            {
                return 8;
            }
            else if (daysDifference ==2)
            {
                return 6;
            }
            else if (daysDifference ==3)
            {
                return 4;
            }
            else if (daysDifference ==4)
            {
                return 2;
            }
           return 0;
        }

        public List<MatchResultDto> GetMatches(int lostItemId , int userId)
        {

            var results = new List<MatchResultDto>();
            var lostItem = _lostRepository.GetLostItemById(lostItemId, userId);
         
            if (lostItem == null)
            {
              throw new KeyNotFoundException($"Lost item with ID {lostItemId} not found.");
            }
            var foundItems= _foundRepository.GetAllFoundItems();

            foreach(var foundItem in foundItems)
            {
                int score = 0;










                score += CalculateWordScore(lostItem.ItemName, foundItem.ItemName, 40);
                score += CalculateWordScore(lostItem.Location, foundItem.Location, 30);
                score += CalculateWordScore(lostItem.Description, foundItem.Description, 20);  
                score += calculateDateScore(lostItem.DateLost, foundItem.DateFound);
                if (score >= 40)
                {
                    var match= new MatchResultDto()
                    {
                    FoundItemId = foundItem.Id,
                    ItemName = foundItem.ItemName,
                    Description = foundItem.Description,
                    Location = foundItem.Location,
                    DateFound = foundItem.DateFound,
                    MatchPercentage = score,
                    ImageUrl = foundItem.ImageUrl
                        };
                    results.Add(match);
                } 

            }
            return results.OrderByDescending(x=> x.MatchPercentage).ToList();


        }
    }
}