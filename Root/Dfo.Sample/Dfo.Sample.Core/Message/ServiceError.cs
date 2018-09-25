using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfo.Sample.Core.Message
{
    public class ServiceError
    { 
        public string ErrorCode { get; set; }
        public string Description { get; set; }
        public string FieldName { get; set; }
        public string Message { get; set; }
        public string MessageId { get; set; }
        public string Params { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
