using Messaging;

public class GerenciadorPerfis
{
    public PerfilJogadorAtivo PerfilAtivo { get; private set; }

    public void AtivarPerfilMaquina()
    {
        Messenger.Send(MessageType.PerfilJogadorAtivado, new Message<PerfilJogadorAtivo>(PerfilJogadorAtivo.Maquina));
    }

    public void AtivarPerfilJogador()
    {
        Messenger.Send(MessageType.PerfilJogadorAtivado, new Message<PerfilJogadorAtivo>(PerfilJogadorAtivo.Jogador));
    }
}