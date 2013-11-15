using System.Collections.Generic;

namespace SSaME.Core.Sequenciador
{
    public interface IArena
    {
        List<IPersonagem> TimeA { get; }
        List<IPersonagem> TimeB { get; }

        void Adicionar(IPersonagem personagem);
    }
}