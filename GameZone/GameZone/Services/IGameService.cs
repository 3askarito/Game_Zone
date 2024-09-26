using GameZone.Models;

namespace GameZone.Services
{
    public interface IGameService
    {
        Task Create(CreateGameFormViewModel model);
        IEnumerable<Game> GetAll();
        Game GetById(int id);
         Task<Game?> Update(EditGameFormViewModel model);
        bool Delete(int id);
    }
}
