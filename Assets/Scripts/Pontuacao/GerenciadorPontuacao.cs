using UnityEngine;

public class GerenciadorPontuacao
{
    public int Pontuacao { get; private set; }
    public int Rodada { get; private set; }

    public void Pontuar()
    {
        Rodada++;
        Pontuacao += CalcularPontosJogada();
    }

    public void ZerarPontuacao()
    {
        Pontuacao = 0;
        Rodada = 0;
    }

    private int CalcularPontosJogada()
    {
        return (int)(Rodada + Mathf.Pow(Rodada, 2) / 10f);
    }
}