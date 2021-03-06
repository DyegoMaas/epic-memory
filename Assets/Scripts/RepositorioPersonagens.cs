using System.Collections.Generic;
using System.Linq;
using EpicMemory.Sequenciador;

namespace Assets.Scripts
{
    public class RepositorioPersonagens : IArena
    {
        private readonly Dictionary<int, IPersonagem> Personagens = new Dictionary<int, IPersonagem>();
        public List<IPersonagem> TimeA { get; private set; }
        public List<IPersonagem> TimeB { get; private set; }
    
        private static int idAtual;

        public RepositorioPersonagens()
        {
            TimeA = new List<IPersonagem>();
            TimeB = new List<IPersonagem>();
        }

        public void Adicionar(IPersonagem personagem)
        {
            personagem.Inicializar(ProximoId());
            Personagens.Add(personagem.Id, personagem);

            var time = (personagem.Equipe == Equipe.A) ? TimeA : TimeB;
            time.Add(personagem);
        }

        public List<IPersonagem> BuscarTodos()
        {
            return Personagens.Values.ToList();
        }

        public void ResetarNivelPersonagens()
        {
            foreach (var personagem in Personagens.Values)
            {
                personagem.ResetarNivel();
            }
        }

        private static int ProximoId()
        {
            return ++idAtual;
        }
    }
}