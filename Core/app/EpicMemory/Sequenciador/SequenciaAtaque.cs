using System.Collections.Generic;

namespace EpicMemory.Sequenciador
{
    public class SequenciaAtaque  
    {
        private readonly List<Ataque> ataquesReproducao = new List<Ataque>(); 

        public void ArmazenarAtaque(Ataque ataque)
        {
            ataquesReproducao.Add(ataque);
        }

        public void RemoverAtaque(Ataque ataque)
        {
            if (ataquesReproducao.Contains(ataque))
                ataquesReproducao.Remove(ataque);
        }

        public bool Validar(List<Ataque> ataquesGravacao)
        {
            if (ataquesReproducao.Count > ataquesGravacao.Count)
                return false;

            for (int i = 0; i < ataquesReproducao.Count; i++)
            {
                var ataqueGravado = ataquesGravacao[i];
                var ataqueReproducao = ataquesReproducao[i];

                if (ataqueGravado.Atacante.Equipe != ataqueReproducao.Atacante.Equipe)
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
}