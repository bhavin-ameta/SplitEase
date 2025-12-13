namespace SplitEase.Services
{
    public class ApiResponse<T>
    {
        public string? Message {  get; set; }
        
        public T? Data { get; set; }

        public bool  IsSuccess { get; set; }



        public ApiResponse(bool isSuccess, string message, T? data = default)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }
        public static ApiResponse<T> Success(string message, T? data = default)
        {
            return new ApiResponse<T>(true, message, data);
        }

        public static ApiResponse<T> Fail(string message)
        {
            return new ApiResponse<T>(false, message);
        }

    }
}
