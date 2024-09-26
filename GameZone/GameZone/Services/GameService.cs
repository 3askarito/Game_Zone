
using GameZone.Models;
using GameZone.Settings;
using Microsoft.Extensions.Options;

namespace GameZone.Services
{
    public class GameService : IGameService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;

        public GameService(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            dbContext = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.imagePath}";
        }
        public async Task Create(CreateGameFormViewModel model)
        {
            var coverName = await SaveCover(model.Cover);
            Game game = new()
            {
                Name = model.Name,
                Description = model.Description,
                CategoryId = model.CategoryId,
                Cover = coverName,
                Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d}).ToList()
            };
            dbContext.Add(game);
            dbContext.SaveChanges();
        }

        public IEnumerable<Game> GetAll()
        {
            var games = dbContext.Games.Include(g => g.Category).Include(g => g.Devices).ThenInclude(d => d.Device).AsNoTracking().ToList();
            return (games);
        }
        public Game? GetById(int id)
        {
            var game = dbContext.Games.Include(g => g.Category).Include(g => g.Devices).ThenInclude(d => d.Device).AsNoTracking().SingleOrDefault(g => g.Id == id);
            return (game);
        }

        public async Task<Game?> Update(EditGameFormViewModel model)
        {
            var game = dbContext.Games.Include(g => g.Devices).SingleOrDefault(d => d.Id == model.Id);
            if (game == null)
                return null;
            var hasNewCover = model.Cover is not null;
            var currentCover = game.Cover;

            game.Name = model.Name;
            game.Description = model.Description;
            game.CategoryId = model.CategoryId;
            game.Devices = model.SelectedDevices.Select(d => new GameDevice { DeviceId = d }).ToList();

            if (hasNewCover)
            {
                game.Cover = await SaveCover(model.Cover!);
            }
            var effectedRows = dbContext.SaveChanges();
            if(effectedRows > 0)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imagesPath, currentCover);
                    File.Delete(cover);
                }
                return game;
            }
            else
            {
                var cover = Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);
                return null;
            }
        }
        public bool Delete(int id)
        {
            var isDeleted = false;
            var game = dbContext.Games.Find(id);
            if (game is null)
                return isDeleted;
            dbContext.Games.Remove(game);
            var effectedRows= dbContext.SaveChanges();
            if (effectedRows > 0)
            {
                isDeleted = true;
                var cover= Path.Combine(_imagesPath, game.Cover);
                File.Delete(cover);
            }
            return isDeleted;
        }
        private async Task<string> SaveCover(IFormFile cover)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}";
            var path = Path.Combine(_imagesPath, coverName);
            using var stream = File.Create(path);
            await cover.CopyToAsync(stream);
            return coverName;
        }
    }
}
