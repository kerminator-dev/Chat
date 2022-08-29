using Chat.API.DTOs;
using Chat.API.Entities;

namespace Chat.API.Services.DialogueRepositories
{
    public interface IDialogueRepository
    {
        Task<ICollection<Dialogue>> GetAll(User user);

        Task<Dialogue> Get(User user, int id);

        Task Create(Dialogue dialogue);

        Task AddMessage(Dialogue dialogue, DialogueMessage message);

        Task<bool> Any(User user1, User user2);

        Task<ICollection<Dialogue>> GetDialoguesWithLastMessages(User user);
    }
}
