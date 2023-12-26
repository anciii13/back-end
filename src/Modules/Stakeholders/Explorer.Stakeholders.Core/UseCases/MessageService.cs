﻿using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.API.Dtos;
using Explorer.Stakeholders.API.Public;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Stakeholders.Core.UseCases
{
    public class MessageService : CrudService<MessageDto, Message>, IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISocialProfileRepository _socialProfileRepository;
        private readonly INotificationRepository _notificationRepository;

        public MessageService(IMessageRepository messageRepository, IUserRepository userRepository, ISocialProfileRepository socialProfileRepository, INotificationRepository notificationRepository, IMapper mapper) : base(messageRepository, mapper)
        {
            _messageRepository = messageRepository;
            _userRepository = userRepository;
            _socialProfileRepository = socialProfileRepository;
            _notificationRepository = notificationRepository;
        }

        public Result<MessageDto> Send(MessageDto messageDto)
        {
            if (!CanSend(messageDto) || messageDto.SenderId == messageDto.RecipientId)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError("You cannot send message.");
            }
            messageDto.SenderUsername = _userRepository.GetUserById(messageDto.SenderId).Username;
            messageDto.RecipientUsername = _userRepository.GetUserById(messageDto.RecipientId).Username;
            var message = _messageRepository.Send(MapToDomain(messageDto));
            var notification = _notificationRepository.CreateMessageNotification("New message: " + message.Title, message.RecipientId, message.Id);

            return MapToDto(message);
        }

        public Result<List<MessageDto>> GetAllSent(int userId)
        {
            var sentMessages = _messageRepository.GetAllSent(userId);
            return MapToDto(sentMessages);
        }

        public Result<List<MessageDto>> GetAllReceived(int userId)
        {
            var receivedMessages = _messageRepository.GetAllReceived(userId);
            return MapToDto(receivedMessages);
        }

        public Result<List<MessageDto>> GetAllUnread(int userId)
        {
            var unreadMessages = _messageRepository.GetAllUnread(userId);
            return MapToDto(unreadMessages);
        }



        private bool CanSend(MessageDto messageDto)
        {
            var isSenderExists = _userRepository.GetAll().Any(u => u.Id == messageDto.SenderId);
            var isRecipientExists = _userRepository.GetAll().Any(u => u.Id == messageDto.RecipientId);
            var senderSocialProfile = _socialProfileRepository.Get(messageDto.SenderId);
            var isFollower = senderSocialProfile.IsFollower(messageDto.RecipientId);

            return true; /*isSenderExists && isRecipientExists && isFollower*/;
        }
        public Result<MessageDto> MarkAsRead(int id)
        {
            var readMessage = _messageRepository.MarkAsRead(id);
            return MapToDto(readMessage);
        }

        public Result<List<List<MessageDto>>> GetChats(int userId)
        {
            var chats = _messageRepository.GetChats(userId);
            
            /*
            var chatsDto = new Result<List<List<MessageDto>>>();

            foreach (var chat in chats)
            {
                var chatDto = MapToDto(chat);
                chatsDto.Value.Add(chatDto.Value);
            }
            return chatsDto;
            */
            return MapToDto(chats);
        }
    }
}
