using System.Collections.Generic;

public class Sequencia
{
    private readonly List<Ataque> ataquesReproducao = new List<Ataque>();

    public void ArmazenarAtaque(Ataque ataque)
    {
        ataquesReproducao.Add(ataque);
    }

    public bool Validar(List<Ataque> ataquesGravacao)
    {
        if (ataquesReproducao.Count > ataquesGravacao.Count)
            return false;

        for (int i = 0; i < ataquesReproducao.Count; i++)
        {
            var ataqueGravado = ataquesGravacao[i];
            var ataqueReproducao = ataquesReproducao[i];

            if (ataqueGravado.TimeAtacante != ataqueReproducao.TimeAtacante)
                return false;

            if (ataqueGravado.Atacante != ataqueReproducao.Atacante)
                return false;

            if (ataqueGravado.Alvo != ataqueReproducao.Alvo)
                return false;
        }

        return true;
    }

    public bool EstaCompleta(List<Ataque> ataquesGravacao)
    {
        return ataquesGravacao.Count == ataquesReproducao.Count;
    }
}