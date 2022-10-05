namespace DTO.General.Base.Api.Output
{
    public class BaseApiOutput
    {
        public BaseApiOutput(bool scs) => Success = scs;
        public BaseApiOutput(bool scs, string msg)
        {
            Message = msg;
            Success = scs;
        }
        public BaseApiOutput(string msg)
        {
            Message = msg;
            Success = false;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
