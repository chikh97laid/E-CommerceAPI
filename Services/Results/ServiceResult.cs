namespace OnlineStore.Services.Results
{
    public class ServiceResult<T>
    {
        public bool Success { get; set; } // is opt successful?
        public string? ErrorMessage { get; set; } // if the opt failed , what's the message
        public T? Data { get; set; } // the result if the opt is success

        public static ServiceResult<T> Ok(T data) => new() {Success = true, Data = data }; // to create successful result

        // to create failed result
        public static ServiceResult<T> Fail(string errorMessage) => new() {Success = false , ErrorMessage = errorMessage};
    }
}
