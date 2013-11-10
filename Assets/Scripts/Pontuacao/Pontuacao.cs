using Messaging;
using UnityEngine;

public class Pontuacao : MonoBehaviour
{
    private int pontuacao;

    // Use this for initialization
    void Start()
    {
        Messenger.Subscribe(MessageType.GameOver, gameObject, "ZerarPontuacao");
        Messenger.Subscribe(MessageType.JogadaCompleta, gameObject, "Pontuar");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ZerarPontuacao()
    {
        DefinirPontuacao(0);
    }

    void Pontuar()
    {
        DefinirPontuacao(CalcularPontuacao());
    }

    private void DefinirPontuacao(int pontos)
    {
        pontuacao = pontos;
        BroadcastMessage("AtualizarPontuacao", pontuacao);
    }

    private int CalcularPontuacao()
    {
        return pontuacao + 1;
    }
}
