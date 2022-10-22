namespace Chat.API.Models
{
    public class ResponseOf<TModel, TError>
    {
        protected readonly TModel? _result;
        protected readonly ICollection<TError>? _errors;

        /// <summary>
        /// Результат выполнения/ответ
        /// </summary>
        public virtual TModel? Result => _result;

        /// <summary>
        /// Ошибки
        /// </summary>
        public virtual ICollection<TError>? Errors => _errors;

        /// <summary>
        /// Есть ли ошибки
        /// </summary>
        public virtual bool HasErrors => _errors != null && _errors.Any();

        /// <summary>
        /// Есть ли результат выполнения/ответ
        /// </summary>
        public virtual bool HasResult => _result != null;

        public ResponseOf(TModel? result)
        {
            _result = result;
        }

        public ResponseOf(TError error) : this(new List<TError>() { error })
        {

        }

        public ResponseOf(ICollection<TError>? errors)
        {
            _errors = errors;
        }
    }
}