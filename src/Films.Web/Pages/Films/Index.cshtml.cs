using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Films;

public class IndexModel : PageModel
{
    private readonly IFilmService _filmService;

    public IndexModel(IFilmService filmService)
    {
        _filmService = filmService;
    }

    public IEnumerable<Film> Films { get; set; } = new List<Film>();

    public async Task OnGetAsync()
    {
        Films = await _filmService.GetAllAsync();
    }
}
