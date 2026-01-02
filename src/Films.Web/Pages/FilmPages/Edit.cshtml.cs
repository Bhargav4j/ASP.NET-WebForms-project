using Films.Application.DTOs;
using Films.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.FilmPages;

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
    public FilmEditViewModel Film { get; set; } = new();

    public async Task<IActionResult> OnGetAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var film = await _filmService.GetByIdAsync(id, cancellationToken);
            if (film == null)
            {
                return NotFound();
            }

            Film = new FilmEditViewModel
            {
                Id = film.Id,
                Name = film.Name,
                Description = film.Description,
                IsActive = film.IsActive
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading film for edit");
            return NotFound();
        }
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var updateDto = new FilmUpdateDto
            {
                Name = Film.Name,
                Description = Film.Description,
                IsActive = Film.IsActive
            };

            await _filmService.UpdateAsync(Film.Id, updateDto, cancellationToken);
            _logger.LogInformation("Updated film: {FilmId}", Film.Id);

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film");
            ModelState.AddModelError(string.Empty, "An error occurred while updating the film.");
            return Page();
        }
    }

    public class FilmEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Description { get; set; }

        public bool IsActive { get; set; }
    }
}
