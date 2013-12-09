using System;
using System.Linq;
using System.Reflection;
using Autofac;

public class ReflectionInjection : IDependencyInjector
{
    private readonly IContainer container;

    public ReflectionInjection(IContainer container)
    {
        this.container = container;
    }

    public void Inject(object instance)
    {
        var tipo = instance.GetType();

        //var properties = tipo
        //    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
        //    .Where(prop => Attribute.IsDefined(prop, typeof (InjectedDependencyAttribute))).ToList();
        //foreach (var property in properties)
        //{
        //    property.SetValue(instance, container.Resolve(property.PropertyType), null);
        //}

        var fields = tipo
            .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(prop => Attribute.IsDefined((MemberInfo) prop, typeof(InjectedDependencyAttribute))).ToList();
        foreach (var field in fields)
        {
            field.SetValue(instance, container.Resolve(field.FieldType));
        }
    }
}