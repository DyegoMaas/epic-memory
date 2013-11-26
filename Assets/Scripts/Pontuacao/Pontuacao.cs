using Messaging;
using UnityEngine;

public class Pontuacao : MonoBehaviour
{
    private int pontuacao;
    private int rodada;

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
        rodada = 0;
    }

    void Pontuar()
    {
        rodada++;
        DefinirPontuacao(pontuacao + CalcularPontosJogada());
    }

    private void DefinirPontuacao(int pontos)
    {
        pontuacao = pontos;
        BroadcastMessage("AtualizarPontuacao", pontuacao);
    }

    private int CalcularPontosJogada()
    {
        return (int)(rodada + Mathf.Pow(rodada, 2) / 10f);
    }
}
