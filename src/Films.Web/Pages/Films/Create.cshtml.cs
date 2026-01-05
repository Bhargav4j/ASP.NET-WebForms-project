using Films.Domain.DTOs;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.Films;

public class CreateModel : PageModel
{
    private readonly IFilmService _filmService;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IFilmService filmService, ILogger<CreateModel> logger)
    {
        _filmService = filmService;
        _logger = logger;
    }

    [BindProperty]
    public FilmInputModel Input { get; set; } = new();

    public class FilmInputModel
    {
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

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var createDto = new FilmCreateDto
            {
                Name = Input.Name,
                Description = Input.Description,
                Year = Input.Year,
                Genre = Input.Genre
            };

            await _filmService.CreateAsync(createDto);
            return RedirectToPage("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating film");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the film");
            return Page();
        }
    }
}
