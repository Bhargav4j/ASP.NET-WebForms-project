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
    public FilmEditModel Film { get; set; } = new();

    public class FilmEditModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Year is required")]
        [Range(1800, 2100, ErrorMessage = "Year must be between 1800 and 2100")]
        public int Year { get; set; }

        [StringLength(100, ErrorMessage = "Genre cannot exceed 100 characters")]
        public string? Genre { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var filmDto = await _filmService.GetByIdAsync(id);
            if (filmDto == null)
            {
                return NotFound();
            }

            Film = new FilmEditModel
            {
                Id = filmDto.Id,
                Name = filmDto.Name,
                Description = filmDto.Description,
                Year = filmDto.Year,
                Genre = filmDto.Genre
            };

            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading film for edit: {Id}", id);
            return RedirectToPage("Index");
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
                Name = Film.Name,
                Description = Film.Description,
                Year = Film.Year,
                Genre = Film.Genre
            };

            await _filmService.UpdateAsync(Film.Id, updateDto);
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating film: {Id}", Film.Id);
            ModelState.AddModelError(string.Empty, "An error occurred while updating the film");
            return Page();
        }
    }
}
