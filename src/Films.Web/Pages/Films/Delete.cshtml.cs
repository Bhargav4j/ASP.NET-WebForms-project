using Films.Domain.DTOs;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Films;

public class DeleteModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly ILogger<DeleteModel> _logger;

    public DeleteModel(IFilmService filmService, ILogger<DeleteModel> logger)
    {
        _filmService = filmService;
        _logger = logger;
    }

    [BindProperty]
    public FilmDto? Film { get; set; }

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
            _logger.LogError(ex, "Error loading film for deletion: {FilmId}", id);
            return NotFound();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Film?.Id == null)
        {
            return NotFound();
        }

        try
        {
            await _filmService.DeleteAsync(Film.Id);
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film: {FilmId}", Film.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the film.");
            return Page();
        }
    }
}
