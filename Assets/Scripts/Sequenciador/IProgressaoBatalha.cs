using EpicMemory.Sequenciador;

public interface IProgressaoBatalha
{
    float PercentualCompleto { get; }
    void AtualizarPercentual(SequenciaAtaque sequenciaMaquina, int ataqueAtual);
}