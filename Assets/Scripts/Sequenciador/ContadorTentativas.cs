using Messaging;

public class ContadorTentativas : IContadorTentativas
{
    private int numeroTentativasPorJogo;

    public int NumeroTentativasRestantes { get; private set; }

    public void ConfigurarNumeroTentativas(int numeroTentativas)
    {
        numeroTentativasPorJogo = numeroTentativas;
        Resetar();
    }

    public void Resetar()
    {
        NumeroTentativasRestantes = numeroTentativasPorJogo;
    }

    public void ErroJogador()
    {
        NumeroTentativasRestantes--;
    }
}