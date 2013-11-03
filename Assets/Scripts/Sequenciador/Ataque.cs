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

    public override string ToString()
    {
        return string.Format("{0}({1}) ataca {2}({3})", Atacante, TimeAtacante.ToString(), Alvo, (TimeAtacante == TimeAtacante.TimeA ? TimeAtacante.TimeB : TimeAtacante.TimeA).ToString());
    }
    
}
