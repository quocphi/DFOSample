using System.Threading.Tasks;

namespace Dfo.Sample.Core.ServiceStack
{
    public abstract class BaseActions<TBusiness>
        where TBusiness : class
    {
        protected static TBusiness _businessLogics;

        protected BaseActions(TBusiness businessLogics)
        {
            if (businessLogics != null)
            {
                _businessLogics = businessLogics;
            }
        }

        public virtual async Task<BaseServicesResult> Execute(IDataContext context)
        {
            return await Task.FromResult(new BaseServicesResult());
        }

        public abstract void Init();

        public abstract bool Validate();
    }
}