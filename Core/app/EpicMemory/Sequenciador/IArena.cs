using System.Collections.Generic;

namespace EpicMemory.Sequenciador
{
    public interface IArena
    {
        List<IPersonagem> TimeA { get; }
        List<IPersonagem> TimeB { get; }

        void Adicionar(IPersonagem personagem);
    }
}