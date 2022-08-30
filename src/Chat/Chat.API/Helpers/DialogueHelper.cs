using Chat.API.Entities;

namespace Chat.API.Helpers
{
    public static class DialogueHelper
    {
        /// <summary>
        /// Получить ID второго участника диалога
        /// </summary>
        /// <param name="dialogue">Диалог</param>
        /// <param name="firstMemberId">Id первого участника диалога</param>
        /// <returns>ID второго участника диалога</returns>
        public static int GetSecondDialogueMemberId(Dialogue dialogue, int firstMemberId)
        {
            return dialogue.CreatorId == firstMemberId ? dialogue.MemberId : dialogue.CreatorId;
        }
    }
}
