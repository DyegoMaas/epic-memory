namespace SSaME.Core
{
    public struct Ataque
    {
        public int Atacante;
        public int Alvo;
        public TimeAtacante TimeAtacante;

        public Ataque(int idAtacante, int idAlvo, TimeAtacante timeAtacante)
        {
            Atacante = idAtacante;
            Alvo = idAlvo;
            TimeAtacante = timeAtacante;
        }
    }
}