using Messaging;

public class Pontuacao : InjectionBehaviour
{
    [InjectedDependency]
    private GerenciadorPontuacao gerenciadorPontuacao;
    
    protected override void StartOverride()
    {
        Messenger.Subscribe(MessageType.GameOver, gameObject, "ZerarPontuacao");
        Messenger.Subscribe(MessageType.JogadaCompleta, gameObject, "Pontuar");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ZerarPontuacao()
    {
        gerenciadorPontuacao.ZerarPontuacao();
        FazerBroadcastPontuacao();
    }

    void Pontuar()
    {
        gerenciadorPontuacao.Pontuar();
        FazerBroadcastPontuacao();
    }

    private void FazerBroadcastPontuacao()
    {
        BroadcastMessage("AtualizarPontuacao", gerenciadorPontuacao.Pontuacao);
    }
}