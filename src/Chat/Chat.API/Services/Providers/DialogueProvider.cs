using Chat.API.Entities;
using Chat.API.Models.Responses;
using Chat.API.Services.DialogueRepositories;

namespace Chat.API.Services.Providers
{
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

            var dialoguesResponse = new DialoguesResponse()
            {
                UserId = user.Id,
                Dialogues = dialogues
            };

            return dialoguesResponse;
        }

        public async Task Create(User dialogueCreator, User dialogueMember)
        {
            // Проверка - существует ли уже указанный диалог
            if (await _dialogueRepository.Any(dialogueMember, dialogueCreator))
                return;

            var dialogue = new Dialogue()
            {
                Created = DateTime.UtcNow,
                CreatorId = dialogueCreator.Id,
                MemberId = dialogueMember.Id,
            };

           await _dialogueRepository.Create(dialogue);
        }
    }
}
