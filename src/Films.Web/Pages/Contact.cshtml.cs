using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages;

public class ContactModel : PageModel
{
    public string Message { get; set; } = "Your contact page.";

    public void OnGet()
    {
    }
}
