using GameZone.Attributes;

namespace GameZone.ViewModels
{
    public class EditGameFormViewModel : GameFormViewModel
    {
        public int Id { get; set; }
        public string? CurrentCover {  get; set; }
        [AllowedExtensions(FileSettings.AllwoedExtensions), MaxSize(FileSettings.MaxSizeInBytes)]
        public IFormFile? Cover { get; set; } = default!;
    }
}
