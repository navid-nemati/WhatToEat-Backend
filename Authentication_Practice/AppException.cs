namespace Authentication_Practice
{
    public class AppException : Exception
    {
        public int StatusCode { get; }
        public object? Errors { get; }

        public AppException(string message,
                            int statusCode = StatusCodes.Status400BadRequest,
                            object? errors = null)
            : base(message)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}
