using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public async Task OnGetAsync()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                Films = await _filmService.SearchAsync(SearchTerm);
                _logger.LogInformation("Searched films with term: {SearchTerm}", SearchTerm);
            }
            else
            {
                Films = await _filmService.GetAllAsync();
                _logger.LogInformation("Retrieved all films");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving films");
            TempData["Error"] = "An error occurred while retrieving films.";
            Films = new List<Film>();
        }
    }
}
