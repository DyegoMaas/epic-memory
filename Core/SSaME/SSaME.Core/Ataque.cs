namespace SSaME.Core
{
    public struct Ataque
    {
        public int Atacante;
        public int Alvo;
        public Times TimeAtacante;

        public Ataque(int idAtacante, int idAlvo, Times timeAtacante)
        {
            Atacante = idAtacante;
            Alvo = idAlvo;
            TimeAtacante = timeAtacante;
        }
    }
}