using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;

namespace SocialNetwork.BLL.Services;

public class FriendService
{
    IFriendRepository friendRepository;
    IUserRepository userRepository;

    public FriendService()
    {
        friendRepository = new FriendRepository();
        userRepository = new UserRepository();
    }

    public void AddFriend(FriendRegistrationData friendRegistration)
    {
        if (string.IsNullOrEmpty(friendRegistration.RecipientEmail)) throw new ArgumentNullException();

        var findUserEntity = this.userRepository.FindByEmail(friendRegistration.RecipientEmail) ??
            throw new UserNotFoundException();

        var friendEntity = new FriendEntity()
        {
            user_id = friendRegistration.SenderId,
            friend_id = findUserEntity.id
        };

        if (this.friendRepository.Create(friendEntity) == 0)
            throw new Exception();
    }
}
