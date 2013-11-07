using System.Collections.Generic;

namespace SSaME.Core
{
    public class Arena : IArena
    {
        public List<IPersonagem> TimeA { get; private set; }
        public List<IPersonagem> TimeB { get; private set; }

        private int idAtual;

        public Arena()
        {
            TimeB = new List<IPersonagem>();
            TimeA = new List<IPersonagem>();
        }

        public void AdicionarPersonagem(IPersonagem personagem)
        {
            personagem.Inicializar(ProximoId());

            if (personagem.Time == Time.A)
                TimeA.Add(personagem);
            else
                TimeB.Add(personagem);
        }

        private int ProximoId()
        {
            return ++idAtual;
        }
    }
}