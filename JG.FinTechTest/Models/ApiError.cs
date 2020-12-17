namespace JG.FinTechTest.Models
{
    public class ApiError
    {
        public ApiError(string errorCode, string message)
        {
            this.ErrorCode = errorCode;
            this.Message = message;
        }

        public string ErrorCode { get; set; }

        public string Message { get; set; }
    }
}