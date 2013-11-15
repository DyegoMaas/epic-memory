namespace SSaME.Core.Sequenciador
{
    public class ValidadorAtaques
    {
        public bool AtaqueValido(Ataque ataque)
        {
            return ataque.Atacante.Equipe != ataque.Alvo.Equipe;
        }
    }
}