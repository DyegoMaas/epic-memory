using System.Collections;
using UnityEngine;
using Messaging;

public class BotaoComecarJogo : MonoBehaviour {
    private const float TempoAnimacao = .8f;

    public Vector3 StartPosition;
    public Vector3 MiddlePosition;
    public Vector3 EndPosition;

    // Use this for initialization
    void Start()
    {
        Messenger.Subscribe(MessageType.NovoJogoAguardar, gameObject, "MostrarBotao");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator OnClick()
    {
        yield return StartCoroutine(Esconder());
        Messenger.Send(MessageType.NovoJogoIniciar);
    }

    void MostrarBotao()
    {
        Mostrar();
    }

    void Mostrar()
    {
        transform.position = StartPosition;
        iTween.MoveTo(gameObject, iTween.Hash("position", MiddlePosition, "time", .4f, "easetype", iTween.EaseType.easeOutCubic, "islocal", true));
    }

    IEnumerator Esconder()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", EndPosition, "time", TempoAnimacao, "easetype", iTween.EaseType.easeOutCubic, "islocal", true));
        yield return new WaitForSeconds(TempoAnimacao);
    }
}
