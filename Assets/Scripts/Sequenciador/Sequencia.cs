using System.Collections.Generic;

public class Sequencia
{
    private readonly List<Ataque> ataquesReproducao = new List<Ataque>();

    public void GerarAtaque(Ataque ataque)
    {
        ataquesReproducao.Add(ataque);
    }

    public bool Validar(List<Ataque> ataquesGravacao)
    {
        if (ataquesGravacao.Count != ataquesReproducao.Count)
            return false;

        for (int i = 0; i < ataquesGravacao.Count; i++)
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
}