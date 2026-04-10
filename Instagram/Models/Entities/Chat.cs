namespace Instagram.Models.Entities;

public class Chat
{
    public int ID { get; set; }
    public string FirstID { get; set; }
    public string SecondID { get; set; }
    public DateTime LastDate { get; set; }
    public virtual ICollection<Message> Messages { get; set; }
}
