using Films.Application.DTOs;
using Films.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.FilmPages;

public class DetailsModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly ILogger<DetailsModel> _logger;

    public DetailsModel(IFilmService filmService, ILogger<DetailsModel> logger)
    {
        _filmService = filmService;
        _logger = logger;
    }

    public FilmDto? Film { get; set; }

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            Film = await _filmService.GetByIdAsync(id, cancellationToken);
            if (Film == null)
            {
                return NotFound();
            }

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading film details");
            return NotFound();
        }
    }
}
