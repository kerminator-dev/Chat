namespace Chat.API.Exceptions
{
    /// <summary>
    /// Исключение, вылетаемое при обработке API-методов
    /// </summary>
    public class ProcessingException : Exception
    {
        public ProcessingException(string message) : base(message) { }
    }
}
