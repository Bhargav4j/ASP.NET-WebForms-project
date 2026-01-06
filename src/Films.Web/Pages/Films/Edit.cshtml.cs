using Films.Domain.Entities;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Films.Web.Pages.Films;

public class EditModel : PageModel
{
    private readonly IFilmService _filmService;

    public EditModel(IFilmService filmService)
    {
        _filmService = filmService;
    }

    [BindProperty]
    public FilmInputModel FilmInput { get; set; } = new();

    public class FilmInputModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(1800, 2100)]
        public int Year { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var film = await _filmService.GetByIdAsync(id.Value);

        if (film == null)
        {
            return NotFound();
        }

        FilmInput = new FilmInputModel
        {
            Id = film.Id,
            Name = film.Name,
            Year = film.Year,
            Description = film.Description
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var film = new Film
        {
            Id = FilmInput.Id,
            Name = FilmInput.Name,
            Year = FilmInput.Year,
            Description = FilmInput.Description,
            ModifiedBy = "System"
        };

        await _filmService.UpdateAsync(FilmInput.Id, film);

        return RedirectToPage("./Index");
    }
}
