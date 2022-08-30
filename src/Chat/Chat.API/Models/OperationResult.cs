namespace Chat.API.Models
{
    public class ActionResult<T> where T : class
    {
        public T? Result { get; protected set; }

        public bool Success => (ErrorMessages == null || !ErrorMessages.Any()) && Result != null;

        public ICollection<string>? ErrorMessages { get; protected set; }

        public ActionResult(T result) : this(result, null) 
        {

        }

        public ActionResult(ICollection<string> errorMessages) : this(null, errorMessages) 
        {
            
        }

        public ActionResult(string errorMessage) : this(null, new List<string>() { errorMessage })
        {

        }

        public ActionResult(T? result, ICollection<string>? errorMessages)
        {
            Result = result;
            ErrorMessages = errorMessages;
        }
    }
}
