using EpicMemory.Sequenciador;

namespace EpicMemory.Testes.Unidade.Sequenciador
{
    public class PersonagemFake :IPersonagem
    {
        private int id;
        private readonly Equipe equipe;

        public PersonagemFake(int id, Equipe equipe)
        {
            this.id = id;
            this.equipe = equipe;
        }

        public Equipe Equipe
        {
            get { return equipe; }
        }

        public int Id
        {
            get { return id; }
            set { }
        }

        public int Nivel
        {
            get { return 0; }
        }

        public int Vida
        {
            get { return 0; }
        }

        public void Inicializar(int id)
        {
            this.id = id;
        }

        public void Selecionar()
        {
        }

        public void Atacar()
        {
        }

        public void SubirNivel()
        {
        }

        public void ResetarNivel()
        {
        }

        public void AdicionarVida(int vida)
        {
        }
    }
}