namespace Dfo.Sample.Core.ServiceStack
{
    public interface ICommand
    {
    }

    public interface ICommand<T> : ICommand
    {
        void Execute(T param);
    }

    public interface ICommand<T, TResult>
    {
        TResult Execute(T param);
    }
}