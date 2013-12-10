using Assets.Scripts;
using Autofac;
using EpicMemory.Sequenciador;
using UnityEngine;

public class DependencyInjector : MonoBehaviour, IDependencyInjector
{
    private IContainer container;
    private IDependencyInjector dependencyInjector;

    void Awake()
    {
        container = RegisterDependencies();
        dependencyInjector = new ReflectionInjection(container);

        DontDestroyOnLoad(gameObject);
    }

    private IContainer RegisterDependencies()
    {
        var builder = new ContainerBuilder();

        builder.RegisterType<RepositorioPersonagens>().As<RepositorioPersonagens, IArena>().SingleInstance();
        builder.RegisterType<InputManager>().As<IInputManager>().SingleInstance();
        builder.RegisterType<UnityRandomizer>().As<IRandom>();
        builder.RegisterType<GeradorAtaques>().As<IGeradorAtaques>().SingleInstance();
        builder.RegisterType<GerenciadorPontuacao>().As<GerenciadorPontuacao>().SingleInstance();
        builder.RegisterType<GerenciadorDificuldade>().As<GerenciadorDificuldade>().SingleInstance();
        builder.RegisterType<ValidadorAtaques>().As<ValidadorAtaques>().SingleInstance();
        builder.RegisterType<SequenciaAtaqueFactory>().As<SequenciaAtaqueFactory>().SingleInstance();
        builder.RegisterType<ContadorTentativas>().As<IContadorTentativas>().SingleInstance();
        builder.RegisterType<GerenciadorEstadoJogo>().As<GerenciadorEstadoJogo>().SingleInstance();
        builder.RegisterType<GerenciadorGUI>().As<GerenciadorGUI>().SingleInstance();
        builder.RegisterType<GerenciadorPerfis>().As<GerenciadorPerfis>().SingleInstance();
        builder.RegisterType<ProgressaoAssimetrica>().As<IProgressaoNivelPartida>().SingleInstance();

        return builder.Build();
    }

    public void Inject(object instance)
    {
        dependencyInjector.Inject(instance);
    }
}