using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Messaging;

public class BotaoComecarJogo : InjectionBehaviour, IGuiListener {

    public Vector3 StartPosition;
    public Vector3 MiddlePosition;
    public Vector3 EndPosition;

    [InjectedDependency] private GerenciadorGUI gerenciadorGui;

    protected override void StartOverride()
    {
        gerenciadorGui.Registrar(this);
        //Messenger.Subscribe(MessageType.NovoJogoAguardar, gameObject, "MostrarBotao");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void MostrarBotaoComecar()
    {
        Mostrar();
    }

    IEnumerator OnClick()
    {
        yield return StartCoroutine(Esconder());
        Messenger.Send(MessageType.NovoJogoIniciar);
    }

    //void MostrarBotao()

    //{

    //    Mostrar();

    //}

    void Mostrar()
    {
        transform.position = StartPosition;
        iTween.MoveTo(gameObject, iTween.Hash("position", MiddlePosition, "time", .4f, "easetype", iTween.EaseType.easeOutCubic, "islocal", true));
    }

    IEnumerator Esconder()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", EndPosition, "time", .4f, "easetype", iTween.EaseType.easeOutCubic, "islocal", true));
        yield return new WaitForSeconds(.8f);
    }
}

public class GerenciadorGUI
{
    private readonly List<IGuiListener> listeners = new List<IGuiListener>();
    
    public void Registrar(IGuiListener listener)
    {
        if(!listeners.Contains(listener))
            listeners.Add(listener);
    }

    public void MostrarBotaoComecar()
    {
        listeners.ForEach(l => l.MostrarBotaoComecar());
    }
}

public interface IGuiListener
{
    void MostrarBotaoComecar();
}
