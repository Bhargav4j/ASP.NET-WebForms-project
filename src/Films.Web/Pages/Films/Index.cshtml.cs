using Films.Application.DTOs;
using Films.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Films;

public class IndexModel : PageModel
{
    private readonly IFilmRepository _filmRepository;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(IFilmRepository filmRepository, ILogger<IndexModel> logger)
    {
        _filmRepository = filmRepository;
        _logger = logger;
    }

    public IEnumerable<FilmDto> Films { get; set; } = new List<FilmDto>();

    public async Task OnGetAsync()
    {
        try
        {
            var films = await _filmRepository.GetAllAsync();
            Films = films.Select(f => new FilmDto
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                CreatedDate = f.CreatedDate,
                ModifiedDate = f.ModifiedDate,
                IsActive = f.IsActive
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving films");
        }
    }
}
