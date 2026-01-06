using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.Films;

public class CreateModel : PageModel
{
    private readonly IFilmService _filmService;

    public CreateModel(IFilmService filmService)
    {
        _filmService = filmService;
    }

    [BindProperty]
    public FilmInputModel FilmInput { get; set; } = new();

    public class FilmInputModel
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1800, 2100)]
        public int Year { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }
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

        var film = new Film
        {
            Name = FilmInput.Name,
            Year = FilmInput.Year,
            Description = FilmInput.Description,
            CreatedBy = "System"
        };

        await _filmService.CreateAsync(film);

        return RedirectToPage("./Index");
    }
}
