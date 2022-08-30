namespace Chat.API.Models
{
    public class ActionResult<T> where T : class
    {
        public T? Result { get; protected set; }

        public bool Success => ErrorMessages != null && ErrorMessages.Any();

        public ICollection<string> ErrorMessages { get; protected set; }

        public ActionResult(T result) 
            : this(result, new List<string>()) { }

        public ActionResult(ICollection<string> errorMessages) 
            : this(null, errorMessages) { }

        public ActionResult(T result, string errorMessage)
            : this(result, new List<string>() { errorMessage }) { }

        public ActionResult(T? result, ICollection<string> errorMessages)
        {
            Result = result;
            ErrorMessages = errorMessages;
        }
    }
}
