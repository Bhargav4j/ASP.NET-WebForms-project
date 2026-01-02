using Films.Application.DTOs;
using Films.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.FilmPages;

public class IndexModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IFilmService filmService, ILogger<IndexModel> logger)
    {
        _filmService = filmService;
        _logger = logger;
    }

    public IEnumerable<FilmDto> Films { get; set; } = Enumerable.Empty<FilmDto>();

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public async Task OnGetAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                Films = await _filmService.SearchAsync(SearchTerm, cancellationToken);
                _logger.LogInformation("Searched films with term: {SearchTerm}", SearchTerm);
            }
            else
            {
                Films = await _filmService.GetAllAsync(cancellationToken);
                _logger.LogInformation("Retrieved all films");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving films");
            Films = Enumerable.Empty<FilmDto>();
        }
    }
}
