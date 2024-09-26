namespace GameZone.Services
{
    public class DevicesServices(ApplicationDbContext dbContext) : IDevicesServices
    {
        public IEnumerable<SelectListItem> GetSelectList()
        {
            return dbContext.Devices.Select(d => new SelectListItem { Value = d.Id.ToString(), Text = d.Name }).AsNoTracking().OrderBy(d => d.Text).ToList();
        }
    }
}
