using UnityEngine;
using System.Collections;
using Messaging;

public class JogadorAtivo : MonoBehaviour
{
    public PerfilJogadorAtivo Perfil = PerfilJogadorAtivo.Maquina;

    public Vector3 PosicaoInicial;
    public Vector3 PosicaoFinal;

    public iTween.EaseType EaseType = iTween.EaseType.easeOutBack;

    // Use this for initialization
    void Start()
    {
        Messenger.Subscribe(MessageType.PerfilJogadorAtivado, gameObject, "VerificarAcao");
    }

    // Update is called once per frame
    void Update()
    {
    }

    void VerificarAcao(Message<PerfilJogadorAtivo> mensagem)
    {
        if (mensagem.Value == Perfil)
        {
            DescerElemento();            
        }
        else
        {
            SubirElemento();
        }
    }

    void DescerElemento()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", PosicaoFinal, "time", .4f, "easetype", EaseType, "islocal", true));
    }

    void SubirElemento()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", PosicaoInicial, "time", .4f, "easetype", EaseType, "islocal", true));
    }
}