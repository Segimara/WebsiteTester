namespace WebsiteTester.Application.Models
{
    public class Result<T>
    {
        private T _value;
        private Exception _error;
        private bool _isSuccess;

        private Result(T value)
        {
            _value = value;
            _isSuccess = true;
        }

        private Result(Exception error)
        {
            _error = error ?? throw new ArgumentNullException(nameof(error));
            _isSuccess = false;
        }

        public static Result<T> Success(T value)
        {
            return new Result<T>(value);
        }

        public static Result<T> Failure(Exception error)
        {
            return new Result<T>(error);
        }

        public static implicit operator Result<T>(Exception error)
        {
            return new Result<T>(error);
        }

        public static implicit operator Result<T>(T value)
        {
            return new Result<T>(value);
        }

        public bool IsSuccess => _isSuccess;

        public bool IsFailure => !_isSuccess;

        public T Value
        {
            get
            {
                if (_isSuccess)
                    return _value;
                throw new InvalidOperationException("Cannot access the value of a failed result.");
            }
        }

        public Exception Error
        {
            get
            {
                if (!_isSuccess)
                    return _error;
                throw new InvalidOperationException("Cannot access the error of a successful result.");
            }
        }

        public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Exception, TResult> onFailure)
        {
            if (_isSuccess)
                return onSuccess(_value);
            return onFailure(_error);
        }
    }
}
