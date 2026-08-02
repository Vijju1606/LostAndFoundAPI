using LostAndFoundAPI.DTOs;


namespace LostAndFoundAPI.Services.Interfaces
{
    public interface IMatchService
    {
       List<MatchResultDto> GetMatches(int lostItemId , int userId);
    }
}