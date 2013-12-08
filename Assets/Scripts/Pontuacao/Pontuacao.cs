public class Pontuacao : InjectionBehaviour
{
    [InjectedDependency]
    private GerenciadorPontuacao gerenciadorPontuacao;

    private int pontuacao;
    
    protected override void StartOverride()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (PontuacaoMudou())
        {
            pontuacao = gerenciadorPontuacao.Pontuacao;
            FazerBroadcastPontuacao();
        }
    }

    private bool PontuacaoMudou()
    {
        return pontuacao != gerenciadorPontuacao.Pontuacao;
    }

    private void FazerBroadcastPontuacao()
    {
        BroadcastMessage("AtualizarPontuacao", gerenciadorPontuacao.Pontuacao);
    }
}