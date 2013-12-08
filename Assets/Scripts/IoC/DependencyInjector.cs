using System;
using System.Linq;
using System.Reflection;
using Assets.Scripts;
using Autofac;
using EpicMemory.Sequenciador;
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

        container = builder.Build();
    }

    private void DefineScriptDependencies()
    {
        //dependencyConfiguration.RegisterConfiguration<Sequenciador>(
        //    sequenciador =>
        //    {
        //        sequenciador.repositorioPersonagens = container.Resolve<RepositorioPersonagens>();
        //        sequenciador.inputManager = container.Resolve<IInputManager>();
        //    });
    }

    public void Inject(object instance)
    {
        dependencyConfiguration.Inject(instance);
        InjetarViaReflexao(instance);
    }

    private void InjetarViaReflexao(object instance)
    {
        var tipo = instance.GetType();

        var properties = tipo
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(prop => Attribute.IsDefined(prop, typeof (InjectedDependencyAttribute))).ToList();
        foreach (var property in properties)
        {
            property.SetValue(instance, container.Resolve(property.PropertyType), null);
        }

        var fields = tipo
            .GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(prop => Attribute.IsDefined(prop, typeof(InjectedDependencyAttribute))).ToList();
        foreach (var field in fields)
        {
            field.SetValue(instance, container.Resolve(field.FieldType));
        }
    }
}