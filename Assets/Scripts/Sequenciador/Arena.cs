using System.Collections.Generic;

public class Arena : IArena
{
    public List<int> TimeA { get; private set; }
    public List<int> TimeB { get; private set; }

    private int idAtual;

    public Arena()
    {
        TimeB = new List<int>();
        TimeA = new List<int>();
    }

    public int AdicionarParticipanteAoTimeA()
    {
        int id = ProximoId();
        TimeA.Add(id);

        return id;
    }

    public int AdicionarParticipanteAoTimeB()
    {
        int id = ProximoId();
        TimeB.Add(id);
        return id;
    }

    private int ProximoId()
    {
        return ++idAtual;
    }
}
