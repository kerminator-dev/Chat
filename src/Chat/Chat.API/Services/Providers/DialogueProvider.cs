using Chat.API.DTOs;
using Chat.API.Entities;
using Chat.API.Exceptions;
using Chat.API.Helpers;
using Chat.API.Models.Requests;
using Chat.API.Models.Responses;
using Chat.API.Services.ConnectionRepositories;
using Chat.API.Services.DialogueRepositories;
using Chat.API.Services.MessagingServices;
using Chat.API.Services.UserRepositories;

namespace Chat.API.Services.Providers
{
    // TODO - Обработка и возвращение ошибок

    public class DialogueProvider
    {
        private readonly IConnectionRepository _connectionRepository;
        private readonly IDialogueRepository _dialogueRepository;
        private readonly IMessagingService _messagingService;
        private readonly IUserRepository _userRepository;

        public DialogueProvider(IDialogueRepository dialogueRepository, IMessagingService messagingService, IUserRepository userRepository, IConnectionRepository connectionRepository)
        {
            _dialogueRepository = dialogueRepository;
            _messagingService = messagingService;
            _userRepository = userRepository;
            _connectionRepository = connectionRepository;
        }

        public async Task<DialoguesResponse> GetAll(User user)
        {           
           // Получение списка диалогов пользователя с последними сообщениями
            var dialoguesWithLastMessages = await _dialogueRepository.GetDialoguesWithLastMessages(user);
            if (dialoguesWithLastMessages == null || dialoguesWithLastMessages.Count == 0)
                throw new ProcessingException("Dialogues not found!");

            // Преобразование List<Dialogues> dialoguesWithLastMessages в List<DialogueWithLastMessageDTO>
            var dialogueDTOs = ToDialogMessageDTOs(dialoguesWithLastMessages);

            // Создание DialoguesResponse
            var dialoguesResponse = new DialoguesResponse()
            {
                UserId = user.Id,
                Dialogues = dialogueDTOs
            };

            return dialoguesResponse;
        }

        public async Task Create(User dialogueCreator, User dialogueMember)
        {
            // Проверка - существует ли уже указанный диалог
            if (await _dialogueRepository.Any(dialogueMember, dialogueCreator))
                throw new ProcessingException("Dialog already exist!");

            // Создание модели диалога
            var dialogue = new Dialogue()
            {
                Created = DateTime.UtcNow,
                CreatorId = dialogueCreator.Id,
                MemberId = dialogueMember.Id,
            };

            // Создание диалога в БД
            await _dialogueRepository.Create(dialogue);

            // Уведомление пользователей о создании диалога
            // await _messagingService.Create(dialogueCreator, dialogueMember, dialogue);
        }

        public async Task Delete(User user, DeleteDialogueRequest deleteDialogueRequest)
        {
            // Поиск нужного диалога в списке диалогов пользователя
            var dialogueToDelete = await _dialogueRepository.Get(user, deleteDialogueRequest.DialogueId);
            if (dialogueToDelete == null) // Если такого диалога нет
                throw new ProcessingException("Dialogue not found!");

            // Получение id второго участника диалога
            var dialogueMemberId = DialogueHelper.GetSecondDialogueMemberId(dialogueToDelete, user.Id);

            // Получение второго участника диалога по id
            var dialogueMember = await _userRepository.Get(dialogueMemberId);

            // Удаление диалога из БД
            await _dialogueRepository.Delete(dialogueToDelete);

            if (dialogueMember != null)
            {
                // Подгрузка существующих хаб-подключений пользователя
                await _connectionRepository.LoadConnections(dialogueMember);

                // Уведомление участника диалога о удалении
                // _messagingService.Delete(dialogueMember, dialogueToDelete);
            }

            // Подгрузка существующих хаб-подключений пользователя
            // await _connectionRepository.LoadConnections(user);

            // Уведомление участника диалога о удалении
            // _messagingService.Delete(user, dialogueToDelete);
        }

        private static ICollection<DialogueWithLastMessageDTO> ToDialogMessageDTOs(ICollection<Dialogue> dialogues)
        {
            List<DialogueWithLastMessageDTO> result = new List<DialogueWithLastMessageDTO>();

            foreach (var dialogue in dialogues)
            {
                // Получение последнего сообщения из dialogue.Messages
                var dialogueLastMessage = ToDialogueMessageDTO(dialogue.Messages.FirstOrDefault());

                result.Add
                (
                    new DialogueWithLastMessageDTO()
                    {
                        Created = dialogue.Created,
                        Id = dialogue.Id,
                        CreatorId = dialogue.CreatorId,
                        MemberId = dialogue.MemberId,
                        LastMessage = dialogueLastMessage
                    }
                );
            }

            return result;
        }

        private static DialogueMessageDTO? ToDialogueMessageDTO(DialogueMessage? dialogueMessage)
        {
            if (dialogueMessage == null)
                return null;

            return new DialogueMessageDTO()
            {
                Id = dialogueMessage.Id,
                SenderId = dialogueMessage.SenderId,
                Content = dialogueMessage.Content,
                Created = dialogueMessage.CreatedDate
            };
        }
    }
}
