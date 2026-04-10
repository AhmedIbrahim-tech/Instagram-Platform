using Instagram.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Instagram.ViewComponents;

public class LoginUserAvatarViewComponent : ViewComponent
{
    private readonly UserManager<InstagramUser> _userManager;

    public LoginUserAvatarViewComponent(UserManager<InstagramUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        if (User.Identity?.IsAuthenticated != true)
            return Content(string.Empty);

        var user = await _userManager.GetUserAsync(HttpContext.User);
        return View(user);
    }
}
