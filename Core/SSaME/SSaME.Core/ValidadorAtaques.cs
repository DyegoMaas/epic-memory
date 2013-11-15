using System.Collections.Generic;

namespace SSaME.Core
{
    public class ValidadorAtaques
    {
        public bool AtaqueValido(Ataque ataque)
        {
            return ataque.Atacante.Equipe != ataque.Alvo.Equipe;
        }
    }
}