using System.Collections.Generic;

namespace SSaME.Core
{
    public interface IArena
    {
        List<IPersonagem> TimeA { get; }
        List<IPersonagem> TimeB { get; }

        void AdicionarParticipante(IPersonagem personagem);
    }
}