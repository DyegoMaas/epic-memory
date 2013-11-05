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

    public override string ToString()
    {
        return string.Format("{0}({1}) ataca {2}({3})", Atacante, TimeAtacante.ToString(), Alvo, (TimeAtacante == Times.TimeA ? Times.TimeB : Times.TimeA).ToString());
    }
    
}
