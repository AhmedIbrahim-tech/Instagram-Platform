using Instagram.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace Instagram.Areas.Identity.Data;

public class InstagramUser : IdentityUser
{
    public string Name { get; set; }
    public string Photo { get; set; }
    public string Bio { get; set; }

    public virtual ICollection<Post> Posts { get; set; }
    public virtual ICollection<Story> Stories { get; set; }
}
