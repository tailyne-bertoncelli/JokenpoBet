namespace desafio.DataTransferObject.Generics
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }
        public int? CodeStatus { get; set; }

        public ApiResponse(T data, List<string> errors = null, int? errorCode = null)
        {
            Success = data != null;
            Data = data;
            Errors = errors ?? new List<string>();
            CodeStatus = errorCode;
        }

        public ApiResponse(T data, int? errorCode = null)
        {
            Success = true;
            Data = data;
            Errors = null;
            CodeStatus = errorCode;
        }

        public ApiResponse(List<string> errors = null, int? errorCode = null)
        {
            Success = false;
            Data = default;
            Errors = errors ?? new List<string>();
            CodeStatus = errorCode;
        }
    }
}
