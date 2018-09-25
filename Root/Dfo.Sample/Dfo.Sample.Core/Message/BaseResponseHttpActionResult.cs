using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfo.Sample.Core.Message
{
    public class BaseResponseHttpActionResult<T>
    {
        public T Result { get; set; }
    }
}
