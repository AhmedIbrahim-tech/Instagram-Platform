namespace Instagram.Models.Entities;

public class Follow
{
    public int ID { get; set; }
    public string FollowerID { get; set; }
    public string FollowingID { get; set; }
}
