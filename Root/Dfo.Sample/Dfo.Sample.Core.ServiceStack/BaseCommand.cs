using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfo.Sample.Core.ServiceStack
{
    public abstract class BaseCommand<T> 
        where T : class
    {
        public abstract void Init();

        public abstract bool Validate();
    }
}
