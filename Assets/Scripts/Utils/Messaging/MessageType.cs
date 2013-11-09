namespace Messaging
{
    /// <summary>
    /// Todos os tipos de mensagens suportadas devem ser definidas nesta enumeração
    /// </summary>
    public enum MessageType
    {
        GameOver = 1,
        AtaqueDesferido = 2,
        JogadorSelecionado = 3,
        JogadaCompleta = 4,

        Unknown = 100
    }
}