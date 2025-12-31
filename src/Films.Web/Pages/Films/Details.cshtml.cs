using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Films;

public class DetailsModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IFilmService filmService, ILogger<DetailsModel> logger)
    {
        _filmService = filmService;
        _logger = logger;
    }

    public Film? Film { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            Film = await _filmService.GetByIdAsync(id);

            if (Film == null)
            {
                return NotFound();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading film details for ID {FilmId}", id);
            return NotFound();
        }
    }
}
