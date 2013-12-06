using EpicMemory.Sequenciador;

namespace SSaME.Core.Sequenciador
{
    public class GeradorAtaques   
    {
        private readonly IArena arena;
        private readonly IRandom random;

        public GeradorAtaques(IArena arena, IRandom random)
        {
            this.arena = arena;
            this.random = random;
        }

        public Ataque GerarAtaque()
        {
            int indiceTimeA = random.Range(0, arena.TimeA.Count);
            int indiceTimeB = random.Range(0, arena.TimeB.Count);

            IPersonagem idAtacante;
            IPersonagem idAlvo;
            if (EscolherTimeAtacante() == Equipe.A)
            {
                idAtacante = arena.TimeA[indiceTimeA];
                idAlvo = arena.TimeB[indiceTimeB];
            }
            else
            {
                idAtacante = arena.TimeB[indiceTimeB];
                idAlvo = arena.TimeA[indiceTimeA];
            }

            return new Ataque(idAtacante, idAlvo);
        }

        private Equipe EscolherTimeAtacante()
        {
            return random.Bool() ? Equipe.A : Equipe.B;
        }
    }
}