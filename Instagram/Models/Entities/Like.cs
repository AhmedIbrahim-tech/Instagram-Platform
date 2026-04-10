using Instagram.Areas.Identity.Data;

namespace Instagram.Models.Entities;

public class Like
{
    public int ID { get; set; }
    public string UserID { get; set; }
    public int PostID { get; set; }

    public virtual InstagramUser User { get; set; }
    public virtual Post Post { get; set; }
}
