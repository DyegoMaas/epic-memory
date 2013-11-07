using SSaME.Core;

public class ProgressaoLinear : IProgressaoPartida
{
    private readonly RepositorioPersonagens repositorioPersonagens;
    private readonly int numeroAtaquesParaSubirNivel;
    private int numeroAtaques;

    public ProgressaoLinear(RepositorioPersonagens repositorioPersonagens, int numeroAtaquesParaSubirNivel)
    {
        this.repositorioPersonagens = repositorioPersonagens;
        this.numeroAtaquesParaSubirNivel = numeroAtaquesParaSubirNivel;
    }

    public void AtualizarProgressao(Ataque ataque)
    {
        numeroAtaques++;

        if (numeroAtaques % numeroAtaquesParaSubirNivel == 0)
        {
            SubirNivelTodosPersonagens();
        }
    }

    private void SubirNivelTodosPersonagens()
    {
        var personagens = repositorioPersonagens.BuscarTodos();
        foreach (var personagem in personagens)
        {
            personagem.SubirNivel();
        }
    }
}