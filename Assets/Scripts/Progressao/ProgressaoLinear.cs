using SSaME.Core;

public class ProgressaoLinear : IProgressaoJogo
{
    private readonly int numeroAtaquesParaSubirNivel;
    private int numeroAtaques;

    public ProgressaoLinear(int numeroAtaquesParaSubirNivel)
    {
        this.numeroAtaquesParaSubirNivel = numeroAtaquesParaSubirNivel;
    }

    public void AtualizarProgressao(Ataque ataque)
    {
        numeroAtaques++;

        if (numeroAtaques % numeroAtaquesParaSubirNivel == 0)
        {
            AtualizarNivelPersonagens();
        }
    }

    private static void AtualizarNivelPersonagens()
    {
        var personagens = new RepositorioPersonagens().BuscarTodos();
        foreach (var personagem in personagens)
        {
            personagem.SubirNivel();
        }
    }
}