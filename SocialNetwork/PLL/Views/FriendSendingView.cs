using SocialNetwork.BLL.Exceptions;
using SocialNetwork.BLL.Models;
using SocialNetwork.BLL.Services;
using SocialNetwork.PLL.Helpers;

namespace SocialNetwork.PLL.Views;

public class FriendSendingView
{
    UserService userService;
    FriendService friendService;

    public FriendSendingView(UserService userService, FriendService friendService)
    {
        this.userService = userService;
        this.friendService = friendService;
    }

    public void Show(User user)
    {
        var friendRegistrationData = new FriendRegistrationData();

        Console.WriteLine("Введите почтовый адрес друга: ");
        friendRegistrationData.RecipientEmail = Console.ReadLine();

        friendRegistrationData.SenderId = user.Id;
        friendRegistrationData.FriendId = userService.FindByEmail(friendRegistrationData.RecipientEmail).Id;

        try
        {
            friendService.AddFriend(friendRegistrationData);

            SuccessMessage.Show("Друг успешно добавлен!");
            user = userService.FindById(user.Id);
        }
        catch (UserNotFoundException)
        {
            AlertMessage.Show("Пользователь не найден!");
        }

        catch (ArgumentNullException)
        {
            AlertMessage.Show("Введите корректное значение!");
        }

        catch (Exception)
        {
            AlertMessage.Show("Произошла ошибка при добавлении в друзья!");
        }
    }
}
