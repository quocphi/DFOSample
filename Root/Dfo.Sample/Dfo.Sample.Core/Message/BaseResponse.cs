namespace Dfo.Sample.Core.Message
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public ServiceErrors Errors { get; set; }
    }
}