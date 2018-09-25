namespace Dfo.Sample.Core.Message
{
    public class BaseResponse<T>: BaseResponse
    {
        public T Data { get; set; }
    }
}