using Films.Domain.DTOs;
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
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Range(1800, 2100)]
        public int? Year { get; set; }

        [StringLength(100)]
        public string? Genre { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var film = await _filmService.GetByIdAsync(id);
            if (film == null)
            {
                return NotFound();
            }

            Input = new InputModel
            {
                Id = film.Id,
                Name = film.Name,
                Year = film.Year,
                Genre = film.Genre,
                Description = film.Description
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading film for edit: {FilmId}", id);
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
            var updateDto = new FilmUpdateDto
            {
                Name = Input.Name,
                Year = Input.Year,
                Genre = Input.Genre,
                Description = Input.Description
            };

            await _filmService.UpdateAsync(Input.Id, updateDto);
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film: {FilmId}", Input.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the film.");
            return Page();
        }
    }
}
