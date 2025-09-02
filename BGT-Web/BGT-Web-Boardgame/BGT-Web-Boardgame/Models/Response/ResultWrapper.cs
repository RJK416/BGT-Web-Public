namespace BGT_Web_Boardgame.Models.Response
{
    public class ResultWrapper<T>
    {
        public bool IsSuccess { get; set; }

        public string? Message{ get; set; }

        public T? Result { get; set; }

        public static ResultWrapper<T> Ok(T? result, string? message = null)
        {
            return new ResultWrapper<T>
            {
                IsSuccess = true,
                Message = message,
                Result = result
            };

        }

        public static ResultWrapper<T> Fail(T? result, string? message = null)
        {
            return new ResultWrapper<T>
            {
                IsSuccess = false,
                Message = message
            };
        }



    }
}
