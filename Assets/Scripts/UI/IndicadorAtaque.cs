using SSaME.Core.Sequenciador;
using UnityEngine;
using System.Collections;
using Messaging;

public class IndicadorAtaque : MonoBehaviour
{
    public Equipe Equipe = Equipe.A;

    public Vector3 PosicaoInicial;
    public Vector3 PosicaoFinal;

    public iTween.EaseType EaseType = iTween.EaseType.easeOutBack;

    // Use this for initialization
    void Start()
    {
        Messenger.Subscribe(MessageType.AtaqueDesferido, gameObject, "Reiniciar");
        Messenger.Subscribe(MessageType.GameOver, gameObject, "Reiniciar");
        Messenger.Subscribe(MessageType.ErroJogador, gameObject, "Reiniciar");
        Messenger.Subscribe(MessageType.JogadorSelecionado, gameObject, "SubirElemento");
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Reiniciar()
    {
        yield return new WaitForSeconds(.4f);
        iTween.MoveTo(gameObject, iTween.Hash("position", PosicaoInicial, "time", .4f, "easetype", EaseType, "islocal", true));
    }

    IEnumerator SubirElemento(Message<Equipe> mensagem)
    {
        var equipe = mensagem.Value;
        if (equipe != Equipe)
            yield break;

        iTween.MoveTo(gameObject, iTween.Hash("position", PosicaoFinal, "time", .4f, "easetype", EaseType, "islocal", true));
    }
}
