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

        public bool Validar(SequenciaAtaque outraSequencia)
        {
            var ataquesOutraSequencia = outraSequencia.ataquesReproducao;

            if (ataquesReproducao.Count > ataquesOutraSequencia.Count)
                return false;

            for (int i = 0; i < ataquesReproducao.Count; i++)
            {
                var ataqueGravado = ataquesOutraSequencia[i];
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

        public bool EstaCompleta(SequenciaAtaque outraSequencia)
        {
            return outraSequencia.ataquesReproducao.Count == ataquesReproducao.Count;
        }

        public IList<Ataque> ToList()
        {
            return ataquesReproducao;
        }
    }
}