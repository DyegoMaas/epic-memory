namespace SSaME.Core.Sequenciador
{
    public struct Ataque
    {
        public IPersonagem Atacante;
        public IPersonagem Alvo;

        public Ataque(IPersonagem atacante, IPersonagem alvo)
        {
            Atacante = atacante;
            Alvo = alvo;
        }
    }
}