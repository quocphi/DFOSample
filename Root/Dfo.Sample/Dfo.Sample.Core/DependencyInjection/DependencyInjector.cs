using System;

namespace Dfo.Sample.Core.DependencyInjection
{
    public interface IDependencyInjector
    {
        void Register<I, T>()
            where T : I;
        void Register<I, T>(params object[] parameterValues)
            where T : I;
        void RegisterInstance<T>(T value);
        void RegisterTypes();
        object Resolve(Type type);
        T Resolve<T>();
        T Resolve<T>(params IDependencyParameterExtends[] parameters);
        T Resolve<T>(string name, params IDependencyParameterExtends[] parameters);
        T TryResolve<T>(Func<T> defaultFactory, params IDependencyParameterExtends[] parameters);
        T TryResolve<T>(string name, Func<T> defaultFactory, params IDependencyParameterExtends[] parameters);
    }
}
