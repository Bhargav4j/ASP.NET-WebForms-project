using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.Films;

public class EditModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly ILogger<EditModel> _logger;

    public EditModel(IFilmService filmService, ILogger<EditModel> logger)
    {
        _filmService = filmService;
        _logger = logger;
    }

    [BindProperty]
    public FilmEditViewModel Film { get; set; } = new FilmEditViewModel();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var film = await _filmService.GetByIdAsync(id);

            if (film == null)
            {
                return NotFound();
            }

            Film = new FilmEditViewModel
            {
                Id = film.Id,
                Name = film.Name,
                Year = film.Year,
                Description = film.Description
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading film for edit, ID {FilmId}", id);
            return NotFound();
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var film = new Film
            {
                Id = Film.Id,
                Name = Film.Name,
                Year = Film.Year,
                Description = Film.Description
            };

            await _filmService.UpdateAsync(Film.Id, film);
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film with ID {FilmId}", Film.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the film.");
            return Page();
        }
    }

    public class FilmEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        public int? Year { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }
    }
}
