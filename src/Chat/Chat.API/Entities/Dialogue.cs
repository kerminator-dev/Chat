using System.ComponentModel.DataAnnotations.Schema;

namespace Chat.API.Entities
{
    public class Dialogue
    {
        /// <summary>
        /// ID диалога
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата и время создания диалога
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// ID User'а, создавшего диалог
        /// </summary>
        public int CreatorId { get; set; }

        /// <summary>
        /// ID User'а, участника диалога
        /// </summary>
        public int MemberId { get; set; }

        public virtual List<DialogueMessage> Messages { get; set; }

       // public virtual User Creator { get; set; }
       // public virtual User Member { get; set; }
    }
}
