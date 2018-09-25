using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfo.Sample.Core.Message
{
    public class BaseRequest
    {
        public string BrowserSessionId { get; set; }
        public string LoginID { get; set; }
    }
}
