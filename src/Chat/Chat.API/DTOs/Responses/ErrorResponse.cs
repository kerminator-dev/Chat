namespace Chat.API.DTOs.Responses
{
    public class ErrorResponse<TError>
    {
        protected TError[] _errors;
        public TError[] Errors => _errors;

        public ErrorResponse(IEnumerable<TError> errors)
            : this( errors.ToArray() )
        {

        }

        public ErrorResponse(TError[] errors)
        {
            _errors = errors;
        }

        public ErrorResponse(TError error) 
            : this(new TError[] { error })
        {

        }
    }
}
