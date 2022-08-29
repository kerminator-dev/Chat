using Chat.API.DTOs;
using Chat.API.Entities;
using Chat.API.Exceptions;
using Chat.API.Models.Responses;
using Chat.API.Services.DialogueRepositories;

namespace Chat.API.Services.Providers
{
    // TODO - Обработка и возвращение ошибок

    public class DialogueProvider
    {
        private readonly IDialogueRepository _dialogueRepository;

        public DialogueProvider(IDialogueRepository dialogueRepository)
        {
            _dialogueRepository = dialogueRepository;
        }

        public async Task<DialoguesResponse> GetAll(User user)
        {
            var dialogues = await _dialogueRepository.GetAll(user);
            if (dialogues == null || dialogues.Count == 0)
                throw new ProcessingException("Dialogues not found!");

            var dialoguesWithLastMessages = await _dialogueRepository.GetDialoguesWithLastMessages(user);
            if (dialoguesWithLastMessages == null || dialoguesWithLastMessages.Count == 0)
                throw new ProcessingException("Dialogues not found!");

            var dialoguesResponse = new DialoguesResponse()
            {
                UserId = user.Id,
                Dialogues = ToDialogsWithMessages(dialoguesWithLastMessages)
            };

            return dialoguesResponse;
        }

        public async Task Create(User dialogueCreator, User dialogueMember)
        {
            // Проверка - существует ли уже указанный диалог
            if (await _dialogueRepository.Any(dialogueMember, dialogueCreator))
                throw new ProcessingException("Dialog already exist!");

            var dialogue = new Dialogue()
            {
                Created = DateTime.UtcNow,
                CreatorId = dialogueCreator.Id,
                MemberId = dialogueMember.Id,
            };

           await _dialogueRepository.Create(dialogue);
        }

        private ICollection<DialogueWithLastMessageDTO> ToDialogsWithMessages(ICollection<Dialogue> dialogues)
        {
            List<DialogueWithLastMessageDTO> result = new List<DialogueWithLastMessageDTO>();

            foreach (var dialogue in dialogues)
            {
                result.Add
                (
                    new DialogueWithLastMessageDTO()
                    {
                        Created = dialogue.Created,
                        Id = dialogue.Id,
                        CreatorId = dialogue.CreatorId,
                        MemberId = dialogue.MemberId,
                        LastMessage = ToDialogueMessageDTO(dialogue.Messages.FirstOrDefault())
                    }
                );
            }

            return result;
        }

        private DialogueMessageDTO ToDialogueMessageDTO(DialogueMessage dialogueMessage)
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
