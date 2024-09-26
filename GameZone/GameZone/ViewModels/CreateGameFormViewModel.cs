using GameZone.Attributes;
namespace GameZone.ViewModels
{
    public class CreateGameFormViewModel : GameFormViewModel
    {
        [AllowedExtensions(FileSettings.AllwoedExtensions), MaxSize(FileSettings.MaxSizeInBytes)]
        public IFormFile Cover { get; set; } = default!;
    }
}
