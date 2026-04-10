using Microsoft.AspNetCore.Mvc;

namespace Instagram.Controllers;

public class PostsController : Controller
{
    [HttpPost]
    public IActionResult UploadPost(IFormFile[] file, string desc)
    {
        return View();
    }
}
