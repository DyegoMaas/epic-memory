using Assets.Scripts;
using SSaME.Core.Sequenciador;

public class ProgressaoAssimetrica : IProgressaoPartida
{
    private RepositorioPersonagens repositorioPersonagens;

    public ProgressaoAssimetrica(RepositorioPersonagens repositorioPersonagens)
    {
        this.repositorioPersonagens = repositorioPersonagens;
    }

    public void AtualizarProgressao(Ataque ataque)
    {
        ataque.Atacante.SubirNivel();
    }

    public void ResetarProgressoPartida()
    {
        repositorioPersonagens.ResetarNivelPersonagens();
    }
}