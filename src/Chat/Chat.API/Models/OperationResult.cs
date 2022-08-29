namespace Chat.API.Models
{
    public class OperationResult<T>
    {
        public T Result { get; set; }

        public bool Success => ErrorMessages != null && ErrorMessages.Any();

        public ICollection<string> ErrorMessages { get; set; }

        public OperationResult(T result, ICollection<string> errorMessages)
        {
            Result = result;
            ErrorMessages = errorMessages;
        }
    }
}
