using Messaging;
using UnityEngine;

public class EstadoJogoScript : InjectionBehaviour
{
    [InjectedDependency] private GerenciadorEstadoJogo gerenciadorEstadoJogo;

    protected override void StartOverride()
    {
        Messenger.Subscribe(MessageType.NovoJogoIniciar, gameObject, "IniciarNovoJogo");
    }

    private void IniciarNovoJogo()
    {
        gerenciadorEstadoJogo.IniciarNovoJogo();
    }

    // Update is called once per frame
	void Update () {
	
	}
}
