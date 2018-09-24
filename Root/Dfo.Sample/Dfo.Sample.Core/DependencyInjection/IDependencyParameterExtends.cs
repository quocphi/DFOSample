namespace Dfo.Sample.Core.DependencyInjection
{
    public interface IDependencyParameterExtends
    {
        string ParameterName { get; set; }
        object ParameterValue { get; set; }
    }
}