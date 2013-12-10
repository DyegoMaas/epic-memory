using EpicMemory.Sequenciador;

public class ProgressaoBatalha : IProgressaoBatalha
{
    public float PercentualCompleto { get; private set; }

    public void AtualizarPercentual(SequenciaAtaque sequenciaMaquina, int ataqueAtual)
    {
        PercentualCompleto = ataqueAtual / (float)sequenciaMaquina.ToList().Count;
    }
}