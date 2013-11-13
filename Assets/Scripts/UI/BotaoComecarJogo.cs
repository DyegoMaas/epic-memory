using UnityEngine;
using Messaging;

public class BotaoComecarJogo : MonoBehaviour {
    
    // Use this for initialization
    void Start()
    {
        Messenger.Subscribe(MessageType.NovoJogoAguardar, gameObject, "MostrarBotao");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        Messenger.Send(MessageType.NovoJogoIniciar);
        HabilitarFilhos(false);
    }

    void MostrarBotao()
    {
        HabilitarFilhos(true);   
    }

    void HabilitarFilhos(bool ativo)
    {
        transform.GetChild(0).gameObject.SetActive(ativo);
    }
}
