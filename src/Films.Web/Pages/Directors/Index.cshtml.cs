using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Directors;

public class IndexModel : PageModel
{
    private readonly IDirectorService _directorService;

    public IndexModel(IDirectorService directorService)
    {
        _directorService = directorService;
    }

    public IEnumerable<Director> Directors { get; set; } = new List<Director>();

    public async Task OnGetAsync()
    {
        Directors = await _directorService.GetAllAsync();
    }
}
