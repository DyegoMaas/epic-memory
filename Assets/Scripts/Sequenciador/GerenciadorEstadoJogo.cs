public class GerenciadorEstadoJogo
{
    private readonly IContadorTentativas contadorTentativas;

    public EstadoJogo EstadoJogo { get; private set; }

    public GerenciadorEstadoJogo(IContadorTentativas contadorTentativas)
    {
        this.contadorTentativas = contadorTentativas;
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
}