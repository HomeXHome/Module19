namespace SocialNetwork.BLL.Models;

public class FriendRegistrationData
{
    public int SenderId { get; set; }
    public int FriendId { get; set; }
    public string RecipientEmail { get; set; }
}
