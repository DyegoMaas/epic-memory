using System.Collections.Generic;

namespace SSaME.Core
{
    public class ValidadorAtaques
    {
        private readonly IArena arena;

        public ValidadorAtaques(IArena arena)
        {
            this.arena = arena;
        }

        public bool AtaqueValido(Ataque ataque)
        {
            if (!JogadoresEstaoNosTimesCorretos(ataque))
            {
                return false;
            }

            if (AtacanteEAlvoSaoDoMesmoTime(ataque, arena.TimeA))
            {
                return false;
            }

            if (AtacanteEAlvoSaoDoMesmoTime(ataque, arena.TimeB))
            {
                return false;
            }

            return true;
        }

        private bool JogadoresEstaoNosTimesCorretos(Ataque ataque)
        {
            var timeAtacante = ataque.TimeAtacante == Times.TimeA ? arena.TimeA : arena.TimeB;
            var timeAlvo = ataque.TimeAtacante == Times.TimeA ? arena.TimeB : arena.TimeA;
            if (!timeAtacante.Contains(ataque.Atacante) || !timeAlvo.Contains(ataque.Alvo))
            {
                return false;
            }
            return true;
        }

        private bool AtacanteEAlvoSaoDoMesmoTime(Ataque ataque, List<int> time)
        {
            return time.Contains(ataque.Atacante) && time.Contains(ataque.Alvo);
        }
    }
}