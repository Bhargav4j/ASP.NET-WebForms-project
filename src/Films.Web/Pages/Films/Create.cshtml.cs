using Films.Application.DTOs;
using Films.Domain.Entities;
using Films.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Films;

public class CreateModel : PageModel
{
    private readonly IFilmRepository _filmRepository;
    private readonly ILogger<CreateModel> _logger;

    public CreateModel(IFilmRepository filmRepository, ILogger<CreateModel> logger)
    {
        _filmRepository = filmRepository;
        _logger = logger;
    }

    [BindProperty]
    public FilmCreateDto Film { get; set; } = new();

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
            var film = new Film
            {
                Name = Film.Name,
                Description = Film.Description,
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                CreatedBy = "System"
            };

            await _filmRepository.AddAsync(film);
            _logger.LogInformation("Film {FilmName} created successfully", film.Name);

            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating film");
            ModelState.AddModelError(string.Empty, "An error occurred while creating the film.");
            return Page();
        }
    }
}
