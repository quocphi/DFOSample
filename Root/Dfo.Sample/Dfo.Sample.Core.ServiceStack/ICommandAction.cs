using Dfo.Sample.Core.Web.Base;
using System.Threading.Tasks;

namespace Dfo.Sample.Core.ServiceStack
{
    public interface ICommandAction
    {
        Task<BaseServicesResult> ExecuteActionAsync(ISessionContext context);
    }
}