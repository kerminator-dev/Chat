using Chat.API.DTOs;
using Chat.API.DTOs.Responses;
using Chat.API.Entities;
using Chat.API.Exceptions;
using Chat.API.Helpers;
using Chat.API.Services.Interfaces;

namespace Chat.API.Services.Providers
{
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

        public async Task<GetDialoguesResponseDTO> GetAll(User user)
        {           
            // Получение списка диалогов пользователя с последними сообщениями
            var dialoguesWithLastMessages = await _dialogueRepository.GetDialoguesWithLastMessages(user.Id);
            // if (dialoguesWithLastMessages == null || dialoguesWithLastMessages.Count == 0)
            //     throw new ProcessingException("Dialogues not found!");

            // Преобразование List<Dialogues> dialoguesWithLastMessages в List<DialogueWithLastMessageDTO>
            var dialogueDTOs = ToDialogMessageDTOs(dialoguesWithLastMessages);

            // Создание DialoguesResponse
            var dialoguesResponse = new GetDialoguesResponseDTO()
            {
                UserId = user.Id,
                Dialogues = dialogueDTOs
            };

            return dialoguesResponse;
        }

        public async Task Create(User dialogueCreator, User dialogueMember)
        {
            // Проверка - существует ли уже указанный диалог
            if (await _dialogueRepository.Any(dialogueMember.Id, dialogueCreator.Id))
                throw new ProcessingException("Dialogue already exist!");

            // Создание модели диалога для БД
            var dialogue = new Dialogue()
            {
                Created = DateTime.UtcNow,
                CreatorId = dialogueCreator.Id,
                MemberId = dialogueMember.Id,
            };

            // Добавление диалога в БД
            var createdDialogue = await _dialogueRepository.Create(dialogue);

            // Создание модели для оповещения участников
            var createdDialogueDTO = new CreatedDialogueDTO()
            {
                Id = createdDialogue.Id,
                Created = createdDialogue.Created,
                CreatorId = createdDialogue.CreatorId,
                MemberId = createdDialogue.MemberId
            };

            // Подгрузка существующих хаб-подключений пользователя и уведомление участника диалога о создании диалога
            dialogueCreator.HubConnections = await _connectionRepository.Get(dialogueCreator.Id);
            await _messagingService.SendCreatedDialogue(dialogueCreator, createdDialogueDTO);

            // Подгрузка существующих хаб-подключений пользователя и уведомление участника диалога о создании диалога
            dialogueMember.HubConnections = await _connectionRepository.Get(dialogueMember.Id);
            await _messagingService.SendCreatedDialogue(dialogueMember, createdDialogueDTO);
        }

        public async Task Delete(User user, int dialogueId)
        {
            // Поиск нужного диалога в списке диалогов пользователя
            var dialogueToDelete = await _dialogueRepository.Get(user.Id, dialogueId);
            if (dialogueToDelete == null) // Если такого диалога нет
                throw new NotFoundException("Dialogue not found!");

            // Получение id второго участника диалога
            var dialogueMemberId = DialogueHelper.GetSecondDialogueMemberId(dialogueToDelete, user.Id);

            // Получение второго участника диалога по id
            var dialogueMember = await _userRepository.Get(dialogueMemberId);

            // Модель для оповещения участников
            var deletedDialogue = new DeletedDialogueDTO()
            {
                Id = dialogueId,
                InitiatorId = user.Id
            };

            // Удаление диалога из БД
            await _dialogueRepository.Delete(dialogueToDelete);

            if (dialogueMember != null)
            {
                // Подгрузка существующих хаб-подключений пользователя и уведомление участника диалога о удалении
                 dialogueMember.HubConnections = await _connectionRepository.Get(dialogueMember.Id);
                 await _messagingService.SendDeletedDialogue(dialogueMember, deletedDialogue);
            }

            // Подгрузка существующих хаб-подключений пользователя и уведомление участника диалога о удалении
            user.HubConnections = await _connectionRepository.Get(user.Id);
            await _messagingService.SendDeletedDialogue(user, deletedDialogue);
        }

        private static ICollection<DialogueWithLastMessageDTO>? ToDialogMessageDTOs(ICollection<Dialogue> dialogues)
        {
            if (dialogues == null)
                return null;

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
                MessageId = dialogueMessage.Id,
                SenderId = dialogueMessage.SenderId,
                Content = dialogueMessage.Content,
                Created = dialogueMessage.CreatedDate
            };
        }
    }
}
