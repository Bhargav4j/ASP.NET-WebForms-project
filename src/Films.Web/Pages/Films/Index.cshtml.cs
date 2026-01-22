using Microsoft.AspNetCore.Mvc.RazorPages;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;

namespace Films.Web.Pages.Films;

public class IndexModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IFilmService filmService, ILogger<IndexModel> logger)
    {
        _filmService = filmService;
        _logger = logger;
    }

    public IEnumerable<Film> Films { get; set; } = new List<Film>();

    public async Task OnGetAsync()
    {
        try
        {
            Films = await _filmService.GetAllAsync();
            _logger.LogInformation("Retrieved {Count} films", Films.Count());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving films");
            Films = new List<Film>();
        }
    }
}
