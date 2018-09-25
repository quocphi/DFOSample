using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfo.Sample.Core.Message
{
    public class BaseResponse
    {
        public bool Success { get; set; }
        public ServiceErrors Errors { get; set; }
    }
}
