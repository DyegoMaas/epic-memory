using System;

namespace SSaME.Core
{
    public class Sequenciador   
    {
        private readonly IArena arena;
        private readonly IRandom random;

        public Sequenciador(IArena arena, IRandom random)
        {
            this.arena = arena;
            this.random = random;
        }

        public Ataque GerarAtaque()
        {
            int indiceTimeA = random.NextInt(0, arena.TimeA.Count);
            int indiceTimeB = random.NextInt(0, arena.TimeB.Count);

            int idAtacante, idAlvo;
            var timeAtacante = EscolherTimeAtacante();
            if (timeAtacante == TimeAtacante.TimeA)
            {
                idAtacante = arena.TimeA[indiceTimeA];
                idAlvo = arena.TimeB[indiceTimeB];
            }
            else
            {
                idAtacante = arena.TimeB[indiceTimeB];
                idAlvo = arena.TimeA[indiceTimeA];
            }

            return new Ataque(idAtacante, idAlvo, timeAtacante);
        }

        private TimeAtacante EscolherTimeAtacante()
        {
            return random.Bool() ? TimeAtacante.TimeA : TimeAtacante.TimeB;
        }
    }
}