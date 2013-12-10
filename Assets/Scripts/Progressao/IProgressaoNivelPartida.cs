using EpicMemory.Sequenciador;

public interface IProgressaoNivelPartida
{
    void AtualizarProgressao(Ataque ataque);
    void ResetarProgressoPartida();
}