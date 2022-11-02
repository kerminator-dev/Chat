using Chat.API.DTOs;
using Chat.API.Entities;

namespace Chat.API.Services.Interfaces
{
    public interface IDialogueRepository
    {
        Task<ICollection<Dialogue>> GetAll(int userId);

        Task<Dialogue> Get(int userId, int id);

        Task<Dialogue> Create(Dialogue dialogue);

        //Task AddMessage(Dialogue dialogue, DialogueMessage message);

        Task<bool> Any(int userId1, int userId2);

        Task Delete(Dialogue dialogue);
        Task<ICollection<Dialogue>> GetDialoguesWithLastMessages(int userId);
    }
}
