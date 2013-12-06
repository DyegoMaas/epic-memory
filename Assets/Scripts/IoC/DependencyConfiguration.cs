using System;
using System.Collections.Generic;

public class DependencyConfiguration
{
    private readonly Dictionary<Type, Action<object>> configurations = new Dictionary<Type, Action<object>>();
    
    public void RegisterConfiguration<T>(Action<T> wiring)
    {
        configurations.Add(typeof(T), Convert(wiring));
    }

    private Action<object> Convert<T>(Action<T> myActionT)
    {
        if (myActionT == null) 
            return null;
        return (o => myActionT((T)o));
    }

    public void Inject(object instance)
    {
        var type = instance.GetType();
        if (configurations.ContainsKey(type))
        {
            var action = configurations[type];
            action(instance);
        }
    }
}