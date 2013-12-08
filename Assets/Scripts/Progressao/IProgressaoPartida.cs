using EpicMemory.Sequenciador;

public interface IProgressaoPartida
{
    void AtualizarProgressao(Ataque ataque);
    void ResetarProgressoPartida();
}