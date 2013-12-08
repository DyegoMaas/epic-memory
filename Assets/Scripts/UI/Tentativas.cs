using UnityEngine;
using System.Collections;
using Messaging;

[RequireComponent(typeof(tk2dTextMesh))]
public class Tentativas : MonoBehaviour {

    public Vector3 StartPosition;
    public Vector3 MiddlePosition;
    public Vector3 EndPosition;

    public iTween.EaseType EaseInType = iTween.EaseType.easeInBack;
    public iTween.EaseType EaseOutType = iTween.EaseType.easeOutBack;

    public float TempoExibicao = 1f;

    private tk2dTextMesh textMesh;

    // Use this for initialization
    void Start()
    {
        Messenger.Subscribe(MessageType.ErroJogador, gameObject, "ManipularErroJogador");
        textMesh = GetComponent<tk2dTextMesh>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ManipularErroJogador(Message<int> mensagem)
    {
        AtualizarTexto(mensagem.Value);
        StartCoroutine(DescerTexto());
    }

    private void AtualizarTexto(int tentativas)
    {
        textMesh.text = string.Format("{0} tentativa{1}", tentativas, (tentativas > 0) ? "s" : "");
        textMesh.Commit();
    }

    IEnumerator DescerTexto()
    {
        transform.position = StartPosition;
        iTween.MoveTo(gameObject, iTween.Hash("position", MiddlePosition, "time", .4f, "easetype", EaseOutType, "islocal", true));
        yield return new WaitForSeconds(TempoExibicao);
        iTween.MoveTo(gameObject, iTween.Hash("position", EndPosition, "time", .4f, "easetype", EaseInType, "islocal", true));
    }
}
