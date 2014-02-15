using Messaging;
using UnityEngine;
using System.Collections;

public class Gira180 : MonoBehaviour {

    public iTween.EaseType EaseType = iTween.EaseType.easeOutCubic;
    public float duracaoGiro = .5f;
    public PerfilJogadorAtivo perfilAtivo = PerfilJogadorAtivo.Maquina;

	// Use this for initialization
	void Start () {
        Messenger.Subscribe(MessageType.PerfilJogadorAtivado, gameObject, "VerificarGiro");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void VerificarGiro(Message<PerfilJogadorAtivo> mensagem)
    {
        if (mensagem.Value != perfilAtivo)
        {
            perfilAtivo = mensagem.Value;
            Girar180();
        }
    }
    
    private void Girar180()
    {
        if(gameObject)
            iTween.RotateBy(gameObject, iTween.Hash("x", .5f, "time", duracaoGiro, "easetype", EaseType, "islocal", true));
    }
}
