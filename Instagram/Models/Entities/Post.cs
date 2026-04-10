using Instagram.Areas.Identity.Data;

namespace Instagram.Models.Entities;

public class Post
{
    public int ID { get; set; }
    public DateTime Date { get; set; }
    public string Photos { get; set; }
    public string Desc { get; set; }
    public int LikeCount { get; set; }
    public string UserID { get; set; }

    public virtual InstagramUser User { get; set; }

    public virtual ICollection<Like> Likes { get; set; }
}
