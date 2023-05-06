using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;
using System.ComponentModel.DataAnnotations;
using SocialNetwork.BLL.Exceptions;

namespace SocialNetwork.BLL.Services;

public class UserService
{
    IUserRepository userRepository;
    MessageService messageService;

    public UserService() 
    {
        userRepository = new UserRepository();
        messageService = new MessageService();
    }

    public UserService(IUserRepository userRepository, MessageService messageService)
    {
        this.userRepository = userRepository;
        this.messageService = messageService;
    }

    public void Register(UserRegistrationData userRegistrationData)
    {
        if (string.IsNullOrEmpty(userRegistrationData.FirstName) ||
            string.IsNullOrEmpty(userRegistrationData.LastName) ||
            string.IsNullOrEmpty(userRegistrationData.Email) ||
            string.IsNullOrEmpty(userRegistrationData.Password) ||
            userRegistrationData.Password.Length < 8 ||
            !new EmailAddressAttribute().IsValid(userRegistrationData.Email) ||
            userRepository.FindByEmail(userRegistrationData.Email) != null)
                throw new ArgumentNullException();

        var userEntity = new UserEntity()
        {
            firstname = userRegistrationData.FirstName,
            lastname = userRegistrationData.LastName,
            password = userRegistrationData.Password,
            email = userRegistrationData.Email
        };

        if (this.userRepository.Create(userEntity) == 0)
            throw new Exception();
    }
    public User ConstructUserModel(UserEntity userEntity)
    {
        var incomingMessages = messageService.GetIncomingMessagesByUserId(userEntity.id);

        var outgoingMessages = messageService.GetOutcomingMessagesByUserId(userEntity.id);

        return new User(userEntity.id,
            userEntity.firstname,
            userEntity.lastname,
            userEntity.password,
            userEntity.email,
            userEntity.photo,
            userEntity.favorite_book,
            userEntity.favorite_movie,
            incomingMessages,
            outgoingMessages);
    }

    public User Authenticate(UserAuthenticationData data)
    {
        var findUserEntity = userRepository.FindByEmail(data.Email) ?? throw new UserNotFoundException();
        if (findUserEntity.password != data.Password) throw new WrongPasswordException();

        return ConstructUserModel(findUserEntity);
    }


    public User FindByEmail(string email)
    {
        var findUserEntity = userRepository.FindByEmail(email);
        return findUserEntity is null ? throw new UserNotFoundException() : ConstructUserModel(findUserEntity);
    }

    public void Update(User user)
    {
        var updateUserEntity = new UserEntity()
        {
            id = user.Id,
            firstname = user.FirstName,
            lastname = user.LastName,
            password = user.Password,
            email = user.Email,
            photo = user.Photo,
            favorite_book = user.FavoriteBook,
            favorite_movie = user.FavoriteMovie
        };

        if (this.userRepository.Update(updateUserEntity) == 0)
            throw new Exception();
    }
    public User FindById(int id)
    {
        var findUserEntity = userRepository.FindById(id);
        if (findUserEntity is null) throw new UserNotFoundException();

        return ConstructUserModel(findUserEntity);
    }
}
