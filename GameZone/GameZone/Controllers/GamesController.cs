namespace GameZone.Controllers
{
    public class GamesController(ICategoriesService categories, IDevicesServices devices, IGameService gameService) : Controller
    {
        public IActionResult Index()
        {
            var game = gameService.GetAll();
            return View(game);
        }
        public IActionResult Details(int id)
        {
            var game = gameService.GetById(id);
            if(game == null)
                return NotFound();
            return View(game);
        }
        [HttpGet]
        public IActionResult Create()
        {
            CreateGameFormViewModel viewModel = new()
            {
                Categories = categories.GetSelectList(),
                Devices = devices.GetSelectList()
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(CreateGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = categories.GetSelectList();
                model.Devices = devices.GetSelectList();
                return View(model);
            }
            await gameService.Create(model);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var game = gameService.GetById(id);
            if (game == null)
                return NotFound();
            EditGameFormViewModel viewModel = new()
            {
                Id = id,
                Name = game.Name,
                Description = game.Description,
                CategoryId = game.CategoryId,
                SelectedDevices = game.Devices.Select(g => g.DeviceId).ToList(),
                Categories = categories.GetSelectList(),
                Devices = devices.GetSelectList(),
                CurrentCover = game.Cover
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditGameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = categories.GetSelectList();
                model.Devices = devices.GetSelectList();
                return View(model);
            }
            var game = await gameService.Update(model);
            if (game is null)
                return BadRequest();
            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var isDeleted = gameService.Delete(id);
            return isDeleted? Ok() : BadRequest();
        } 

    }
}
