using NUnit.Framework;
using Moq;
using SocialNetwork.DAL.Repositories;
using SocialNetwork.BLL.Services;
using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Entities;

namespace SocialNetwork.Tests;

[TestFixture]
public class RegistrationTest
{
    [Test]
    public void RegisterWithValidData_CallsCreateMethod()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var messageServiceMock = new Mock<MessageService>();

        var userService = new UserService(userRepositoryMock.Object, messageServiceMock.Object);

        var userRegistrationData = new UserRegistrationData
        {
            FirstName = "Ivan",
            LastName = "Ivan",
            Password = "11112222",
            Email = "gmail3@gmail.com"
        };
        userRepositoryMock.Setup(repo => repo.Create(It.IsAny<UserEntity>())).Returns(1);
        userService.Register(userRegistrationData);
    }

    [Test]
    public void RegisterWithBrokenEmail_ExpectedArgumentNullException()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var messageServiceMock = new Mock<MessageService>();

        var userService = new UserService(userRepositoryMock.Object, messageServiceMock.Object);

        var userRegistrationData = new UserRegistrationData
        {
            FirstName = "Ivan",
            LastName = "Ivan",
            Password = "11112222",
            Email = "123"
        };
        userRepositoryMock.Setup(repo => repo.Create(It.IsAny<UserEntity>())).Returns(1);
        Assert.Throws<ArgumentNullException>(() => userService.Register(userRegistrationData));
    }
    [Test]
    public void RegisterWithSmallPassword_ExpectedArgumentNullException()
    {
        var userRepositoryMock = new Mock<IUserRepository>();
        var messageServiceMock = new Mock<MessageService>();

        var userService = new UserService(userRepositoryMock.Object, messageServiceMock.Object);

        var userRegistrationData = new UserRegistrationData
        {
            FirstName = "Ivan",
            LastName = "Ivan",
            Password = "1",
            Email = "gmail3@gmail.com"
        };
        userRepositoryMock.Setup(repo => repo.Create(It.IsAny<UserEntity>())).Returns(1);
        Assert.Throws<ArgumentNullException>(() => userService.Register(userRegistrationData));
    }
}
