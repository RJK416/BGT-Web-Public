namespace BGT_Web_Account.Helper
{
    public class ResultWrapper<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }

        public T? Result { get; set; }

        public static ResultWrapper<T> Ok(T? result, string? message = null)
        {
            return new ResultWrapper<T>
            {
                Success = true,
                Message = message,
                Result = result
            };

        }

        public static ResultWrapper<T> Fail(T? result, string? message = null)
        {
            return new ResultWrapper<T>
            {
                Success = false,
                Message = message
            };
        }
    }
}
