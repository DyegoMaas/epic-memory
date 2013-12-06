using Autofac;
using UnityEngine;

public class DependencyInjector : MonoBehaviour
{
    private IContainer container;
    private DependencyConfiguration dependencyConfiguration;

    void Awake()
    {
        dependencyConfiguration = new DependencyConfiguration();

        InitializeAutofac();
        DefineScriptDependencies();

        DontDestroyOnLoad(gameObject);
    }

    private void InitializeAutofac()
    {
        var builder = new ContainerBuilder();

        // Autofac registrations go here
        // AsImplementedInterfaces - When you resolve a Player interface
        //    (IPlayer) it will return an instance of Player
        // SingleInstance - All resolves of a Player interface will return
        //    the same instance of Player
        //builder.RegisterType()
        //    .AsImplementedInterfaces()
        //    .SingleInstance();
        builder.RegisterType<TesteA>().As<ITeste>();

        container = builder.Build();
    }

    private void DefineScriptDependencies()
    {
        dependencyConfiguration.RegisterConfiguration<Teste>(t => t.TesteX = container.Resolve<ITeste>());
    }

    public void Inject(object instance)
    {
        dependencyConfiguration.Inject(instance);
    }
}

public interface ITeste
{
    void Testar();
}

public class TesteA : ITeste
{
    public void Testar()
    {
        Debug.Log("XXXXX");
    }
}