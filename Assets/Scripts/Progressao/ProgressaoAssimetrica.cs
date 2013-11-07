using SSaME.Core;

public class ProgressaoAssimetrica : IProgressaoJogo
{
    public void AtualizarProgressao(Ataque ataque)
    {
        ataque.Atacante.SubirNivel();
    }
}