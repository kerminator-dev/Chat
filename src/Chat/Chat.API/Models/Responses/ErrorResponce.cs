namespace Chat.API.Models.Responses
{
    public class ErrorResponce
    {
        public IEnumerable<string> ErrorsMessages { get; set; }

        public ErrorResponce(string errorMessage)
            : this(new List<string>() { errorMessage })
        {

        }

        public ErrorResponce(IEnumerable<string> errorsMessages)
        {
            ErrorsMessages = errorsMessages;
        }
    }
}
