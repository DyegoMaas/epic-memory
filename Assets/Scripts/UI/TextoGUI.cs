using UnityEngine;
using System.Collections;
using Messaging;

public class TextoGUI : MonoBehaviour {

    public Vector3 StartPosition;
    public Vector3 MiddlePosition;
    public Vector3 EndPosition;

    public iTween.EaseType EaseInType = iTween.EaseType.easeInBack;
    public iTween.EaseType EaseOutType = iTween.EaseType.easeOutBack;
    
    public MessageType TipoMensagem = MessageType.Unknown;
    public float TempoExibicao = 1f;


	// Use this for initialization
	void Start () {
        Messenger.Subscribe(TipoMensagem, gameObject, "DescerTexto");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator DescerTexto()
    {
        transform.position = StartPosition;
        iTween.MoveTo(gameObject, iTween.Hash("position", MiddlePosition, "time", .4f, "easetype", EaseOutType));
        yield return new WaitForSeconds(TempoExibicao);
        iTween.MoveTo(gameObject, iTween.Hash("position", EndPosition, "time", .4f, "easetype", EaseInType));
    }
}
