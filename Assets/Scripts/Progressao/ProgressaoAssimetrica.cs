using Assets.Scripts;
using EpicMemory.Sequenciador;

public class ProgressaoAssimetrica : IProgressaoPartida
{
    private readonly RepositorioPersonagens repositorioPersonagens;

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