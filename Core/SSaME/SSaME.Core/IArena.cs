using System.Collections.Generic;

namespace SSaME.Core
{
    public interface IArena
    {
        List<int> TimeA { get; }
        List<int> TimeB { get; }

        int AdicionarParticipanteAoTimeA();
        int AdicionarParticipanteAoTimeB();
    }
}