using EpicMemory.Sequenciador;

public class ProgressaoBatalha : IProgressaoBatalha
{
    public float PercentualCompleto { get; private set; }

    public void AtualizarPercentual(SequenciaAtaque sequenciaMaquina, int ataqueAtual)
    {
        var quantidadeAtaques = sequenciaMaquina.ToList().Count;
        if (quantidadeAtaques == 0)
        {
            PercentualCompleto = 0;
            return;
        }
        PercentualCompleto = ataqueAtual / (float)quantidadeAtaques;
    }
}