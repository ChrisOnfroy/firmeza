using Firmeza.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Web.Controllers;

public class AccountController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager) : Controller
{
    private const string AdministratorRole = "Administrator";
    private const string CustomerRole = "Customer";

    [HttpGet]
    public IActionResult Register() => View(new RegisterViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, CustomerRole);
            return RedirectToAction(nameof(Login));
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(string.Empty, error.Description);
        return View(model);
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null) =>
        View(new LoginViewModel { ReturnUrl = returnUrl });

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        var result = await signInManager.CheckPasswordSignInAsync(
            user, model.Password, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            if (!await userManager.IsInRoleAsync(user, AdministratorRole))
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Customer accounts cannot access the Razor administration panel.");
                return View(model);
            }

            await signInManager.SignInAsync(user, model.RememberMe);
            return LocalRedirect(model.ReturnUrl ?? Url.Action("Index", "Home")!);
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    [HttpGet]
    public IActionResult AccessDenied() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
