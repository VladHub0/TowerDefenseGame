using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class DiContainer
{
    private readonly Dictionary<Type, object> _services = new();

    public void Register<T>(T service)
    {
        _services[typeof(T)] = service;
    }

    public T Resolve<T>() => (T)_services[typeof(T)];

    public object Resolve(Type type) => _services[type];

    public TDeps CreateDeps<TDeps>() where TDeps : IDependencyGroups
    {
        var ctor = typeof(TDeps).GetConstructors().First();
        var args = ctor.GetParameters()
            .Select(p => Resolve(p.ParameterType))
            .ToArray();
        return (TDeps)Activator.CreateInstance(typeof(TDeps), args);
    }

    public void InjectTo(object target)
    {
        var injectInterface = target.GetType()
            .GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IInjects<>));

        if (injectInterface != null)
        {
            var depsType = injectInterface.GetGenericArguments()[0];
            var createMethod = typeof(DiContainer).GetMethod(nameof(CreateDeps))!
                .MakeGenericMethod(depsType);
            var depsInstance = createMethod.Invoke(this, null);
            var injectMethod = injectInterface.GetMethod("Inject");
            injectMethod!.Invoke(target, new[] { depsInstance });
        }
    }
}
