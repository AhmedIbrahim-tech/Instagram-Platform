using Instagram.Areas.Identity.Data;

namespace Instagram.Models.Entities;

public class Seen
{
    public int ID { get; set; }
    public int StoryID { get; set; }
    public string UserID { get; set; }

    public virtual Story Story { get; set; }
    public virtual InstagramUser User { get; set; }
}
