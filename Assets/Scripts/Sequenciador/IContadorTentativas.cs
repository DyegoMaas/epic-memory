public interface IContadorTentativas
{
    int NumeroTentativasRestantes { get; }
    void ConfigurarNumeroTentativas(int numeroTentativas);
    void Resetar();
    void ErroJogador();
}