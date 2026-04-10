using System.Security.Claims;
using Instagram.Areas.Identity.Data;
using Instagram.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Instagram.Services;

public interface IProfileService
{
    Task<InstagramUser> GetInstagramUserAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default);

    Task SaveImageAsync(IFormFile photo, CancellationToken cancellationToken = default);

    Task SaveUserAsync(InstagramUser user, CancellationToken cancellationToken = default);
}

public sealed class ProfileService : IProfileService
{
    private static readonly HashSet<string> AllowedImageExtensions = new(StringComparer.OrdinalIgnoreCase)
    {
        ".jpg", ".jpeg", ".png", ".gif", ".webp"
    };

    private readonly IWebHostEnvironment _environment;
    private readonly UserManager<InstagramUser> _userManager;
    private readonly InstagramContext _db;

    public ProfileService(
        IWebHostEnvironment environment,
        UserManager<InstagramUser> userManager,
        InstagramContext db)
    {
        _environment = environment;
        _userManager = userManager;
        _db = db;
    }

    public Task<InstagramUser> GetInstagramUserAsync(ClaimsPrincipal principal, CancellationToken cancellationToken = default)
    {
        return _userManager.GetUserAsync(principal);
    }

    public async Task SaveImageAsync(IFormFile photo, CancellationToken cancellationToken = default)
    {
        if (photo == null || photo.Length == 0)
            return;

        var ext = Path.GetExtension(photo.FileName);
        if (string.IsNullOrEmpty(ext) || !AllowedImageExtensions.Contains(ext))
            return;

        var uploads = Path.Combine(_environment.WebRootPath, "Uploads");
        Directory.CreateDirectory(uploads);

        var safeFileName = Path.GetFileName(photo.FileName);
        var path = Path.Combine(uploads, safeFileName);

        await using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        await photo.CopyToAsync(stream, cancellationToken);
    }

    public async Task SaveUserAsync(InstagramUser user, CancellationToken cancellationToken = default)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
