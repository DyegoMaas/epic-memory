using System.Collections.Generic;

public interface IArena
{
    List<int> TimeA { get; }
    List<int> TimeB { get; }

    int AdicionarParticipanteAoTimeA();
    int AdicionarParticipanteAoTimeB();
}
