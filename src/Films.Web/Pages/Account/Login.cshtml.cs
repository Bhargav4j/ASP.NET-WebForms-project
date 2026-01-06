using System.ComponentModel.DataAnnotations;
using Films.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Films.Web.Pages.Account;

public class LoginModel : PageModel
{
    private readonly IUserService _userService;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(IUserService userService, ILogger<LoginModel> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [BindProperty]
    [Required(ErrorMessage = "Username is required")]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    public string? SecretAnswer { get; set; }

    [BindProperty]
    [RegularExpression(@"^\d{9}$", ErrorMessage = "Phone number must be exactly 9 digits")]
    public string? PhoneNumber { get; set; }

    public bool ShowPasswordRecovery { get; set; }
    public string? SecretQuestion { get; set; }
    public string? RecoveredPassword { get; set; }

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
            var user = await _userService.AuthenticateAsync(Username, Password);

            if (user != null)
            {
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetInt32("UserId", user.Id);

                _logger.LogInformation("User {Username} logged in successfully", Username);
                return RedirectToPage("/Index");
            }
            else
            {
                TempData["Error"] = "Invalid username or password";
                _logger.LogWarning("Failed login attempt for username {Username}", Username);
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for username {Username}", Username);
            TempData["Error"] = "An error occurred during login. Please try again.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostForgotPasswordAsync()
    {
        try
        {
            var user = await _userService.GetAllAsync();
            var foundUser = user.FirstOrDefault(u => u.Username == Username);

            if (foundUser != null)
            {
                ShowPasswordRecovery = true;
                SecretQuestion = foundUser.SecretQuestion ?? "What is your favorite color?";
                return Page();
            }
            else
            {
                TempData["Error"] = "Username not found";
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password recovery for username {Username}", Username);
            TempData["Error"] = "An error occurred. Please try again.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostRecoverBySecretAsync()
    {
        try
        {
            var user = await _userService.RecoverPasswordBySecretAsync(Username, SecretAnswer ?? string.Empty);

            if (user != null)
            {
                ShowPasswordRecovery = true;
                SecretQuestion = user.SecretQuestion;
                RecoveredPassword = user.Password;
                _logger.LogInformation("Password recovered by secret for username {Username}", Username);
                return Page();
            }
            else
            {
                TempData["Error"] = "Invalid secret answer";
                ShowPasswordRecovery = true;
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password recovery by secret for username {Username}", Username);
            TempData["Error"] = "An error occurred. Please try again.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostRecoverByPhoneAsync()
    {
        try
        {
            var fullPhone = $"+359{PhoneNumber}";
            var user = await _userService.RecoverPasswordByPhoneAsync(Username, fullPhone);

            if (user != null)
            {
                ShowPasswordRecovery = true;
                RecoveredPassword = user.Password;
                _logger.LogInformation("Password recovered by phone for username {Username}", Username);
                return Page();
            }
            else
            {
                TempData["Error"] = "Invalid phone number";
                ShowPasswordRecovery = true;
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password recovery by phone for username {Username}", Username);
            TempData["Error"] = "An error occurred. Please try again.";
            return Page();
        }
    }
}
