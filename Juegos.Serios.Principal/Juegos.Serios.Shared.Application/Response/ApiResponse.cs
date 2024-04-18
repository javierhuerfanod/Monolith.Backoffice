namespace Juegos.Serios.Shared.Application.Response
{
    public class ApiResponse<T>
    {
        public int ResponseCode { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
        public T Data { get; set; }

        public ApiResponse(int responseCode, string message, bool status, T data)
        {
            ResponseCode = responseCode;
            Message = message;
            Status = status;
            Data = data;
        }
    }
}
