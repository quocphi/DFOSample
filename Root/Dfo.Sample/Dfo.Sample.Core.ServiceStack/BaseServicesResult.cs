using System;
using System.Net;

namespace Dfo.Sample.Core.ServiceStack
{
    public class BaseServicesResult
    {
        public HttpStatusCode ReturnCode { get; set; }
        public object ReturnData { get; set; }
        private DateTime _timestamp;
        public DateTime TimeStamp
        {
            get
            {
                _timestamp = DateTime.UtcNow;
                return _timestamp;
            }
            set => _timestamp = value;
        }
    }
}