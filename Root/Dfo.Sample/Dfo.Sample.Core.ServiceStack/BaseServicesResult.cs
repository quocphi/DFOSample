using System;
using System.Net;

namespace Dfo.Sample.Core.ServiceStack
{
    public class BaseServicesResult
    {
        /// <summary>
        /// Gets or sets ReturnCode
        /// </summary>
        public HttpStatusCode ReturnCode { get; set; }

        /// <summary>
        /// Gets or sets ReturnData
        /// </summary>
        public object ReturnData { get; set; }

        private DateTime _timestamp;

        /// <summary>
        /// Gets or sets timeStamp
        /// </summary>
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