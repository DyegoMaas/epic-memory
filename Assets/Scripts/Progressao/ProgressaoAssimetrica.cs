using Assets.Scripts;
using EpicMemory.Sequenciador;

public class ProgressaoAssimetrica : IProgressaoNivelPartida
{
    private readonly RepositorioPersonagens repositorioPersonagens;

    public ProgressaoAssimetrica(RepositorioPersonagens repositorioPersonagens)
    {
        this.repositorioPersonagens = repositorioPersonagens;
    }

    public float PercentualCompleto
    {
        get { throw new System.NotImplementedException(); }
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