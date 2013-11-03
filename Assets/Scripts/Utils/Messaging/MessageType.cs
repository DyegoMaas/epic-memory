using System;

namespace Messaging
{
    /// <summary>
    /// Todos os tipos de mensagens suportadas devem ser definidas nesta enumeração
    /// </summary>
    public enum MessageType
    {
        MessageTypeA = 1,
        MessageTypeB = 2,
        
        // other message types

        Unknown = 100
    }
}