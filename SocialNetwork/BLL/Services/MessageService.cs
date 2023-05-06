using SocialNetwork.BLL.Models;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Repositories;
using System.ComponentModel.DataAnnotations;
using SocialNetwork.BLL.Exceptions;

namespace SocialNetwork.BLL.Services;

public class MessageService
{
    IMessageRepository messageRepository;
    IUserRepository userRepository;

    public MessageService()
    {
        messageRepository = new MessageRepository();
        userRepository = new UserRepository();
    }

    public IEnumerable<Message> GetIncomingMessagesByUserId(int recipientId)
    {
        var messages = new List<Message>();

        messageRepository.FindByRecipientId(recipientId).ToList().ForEach(m =>
        {
            var senderUserEntity = userRepository.FindById(m.sender_id);
            var recipientUserEntity = userRepository.FindById(m.recipient_id);

            messages.Add(new Message(m.id, m.content, senderUserEntity.email, recipientUserEntity.email));
        });
        return messages;
    }

    public IEnumerable<Message> GetOutcomingMessagesByUserId(int senderId)
    {
        var messages = new List<Message>();

        messageRepository.FindByRecipientId(senderId).ToList().ForEach(m =>
        {
            var senderUserEntity = userRepository.FindById(m.sender_id);
            var recipientUserEntity = userRepository.FindById(m.recipient_id);

            messages.Add(new Message(m.id, m.content, senderUserEntity.email, recipientUserEntity.email));
        });
        return messages;
    }

    public void SendMessage(MessageSendingData messageSendingData)
    {
        if (String.IsNullOrEmpty(messageSendingData.Content)) throw new ArgumentNullException();

        if (messageSendingData.Content.Length > 5000) throw new ArgumentOutOfRangeException();

        var findUserEntity = this.userRepository.FindByEmail(messageSendingData.RecipientEmail) ??
            throw new UserNotFoundException();

        var messageEntity = new MessageEntity()
        {
            content = messageSendingData.Content,
            sender_id = messageSendingData.SenderId,
            recipient_id = findUserEntity.id
        };

        if (this.messageRepository.Create(messageEntity) == 0)
            throw new Exception();
    }
}
