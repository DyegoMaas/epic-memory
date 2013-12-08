using System.Collections;
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