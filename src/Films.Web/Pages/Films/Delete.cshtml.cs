using Films.Domain.Entities;
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
    public Film Film { get; set; } = new Film();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var film = await _filmService.GetByIdAsync(id);

            if (film == null)
            {
                return NotFound();
            }

            Film = film;
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading film for delete, ID {FilmId}", id);
            return NotFound();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            await _filmService.DeleteAsync(Film.Id);
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting film with ID {FilmId}", Film.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while deleting the film.");
            return Page();
        }
    }
}
