using SSaME.Core;

public class ProgressaoAssimetrica : IProgressaoPartida
{
    public void AtualizarProgressao(Ataque ataque)
    {
        ataque.Atacante.SubirNivel();
    }
}