public class GerenciadorEstadoJogo
{
    private readonly IContadorTentativas contadorTentativas;
    private readonly GerenciadorPontuacao gerenciadorPontuacao;

    public EstadoJogo EstadoJogo { get; private set; }

    public GerenciadorEstadoJogo(IContadorTentativas contadorTentativas, GerenciadorPontuacao gerenciadorPontuacao)
    {
        this.contadorTentativas = contadorTentativas;
        this.gerenciadorPontuacao = gerenciadorPontuacao;
        EstadoJogo = EstadoJogo.Aguardando;
    }

    public bool FimDeJogo()
    {
        return contadorTentativas.NumeroTentativasRestantes == 0;
    }

    public void IniciarNovoJogo()
    {
        EstadoJogo = EstadoJogo.Iniciado;
    }

    public void AguardarNovoJogo()
    {
        EstadoJogo = EstadoJogo.Aguardando;
    }

    public void PrepararNovoJogo()
    {
        contadorTentativas.Resetar();
        gerenciadorPontuacao.ZerarPontuacao();
    }
}