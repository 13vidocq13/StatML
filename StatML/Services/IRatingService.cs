using StatML.Models;

namespace StatML.Services
{
    public interface IRatingService
    {
        Task<List<PlayerRatingDto>> GetTopArmyRatingsAsync(int maxPages = 9);
        Task SyncAndSaveArmyRatingsAsync(int maxPages = 9); 
    }
}
