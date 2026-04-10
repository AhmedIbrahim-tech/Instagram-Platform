using Instagram.Areas.Identity.Data;
using Instagram.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.Controllers;

[Authorize]
public class ProfilesController : Controller
{
    private readonly IProfileService _profileService;
    private readonly UserManager<InstagramUser> _userManager;

    public ProfilesController(IProfileService profileService, UserManager<InstagramUser> userManager)
    {
        _profileService = profileService;
        _userManager = userManager;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult CreateProfile()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateProfile(string name, string bio, IFormFile photo, CancellationToken cancellationToken)
    {
        if (photo is not { Length: > 0 })
            return View();

        var user = await _profileService.GetInstagramUserAsync(User, cancellationToken);
        if (user == null)
            return Challenge();

        user.Name = name;
        user.Bio = bio;
        user.Photo = photo.FileName;

        await _profileService.SaveImageAsync(photo, cancellationToken);
        await _profileService.SaveUserAsync(user, cancellationToken);

        return View();
    }

    public async Task<IActionResult> MyPageProfile(CancellationToken cancellationToken)
    {
        var user = await _profileService.GetInstagramUserAsync(User, cancellationToken);
        if (user == null)
            return NotFound();

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditProfile(string name, string bio, CancellationToken cancellationToken)
    {
        var user = await _profileService.GetInstagramUserAsync(User, cancellationToken);
        if (user == null)
            return NotFound();

        user.Name = name;
        user.Bio = bio;

        await _profileService.SaveUserAsync(user, cancellationToken);
        return RedirectToAction(nameof(MyPageProfile));
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Unauthorized();

        if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
            return Json(false);

        var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        return Json(result.Succeeded);
    }
}
