namespace SSaME.Core.Testes
{
    public class PersonagemFake :IPersonagem
    {
        private int id;
        private readonly Time time;

        public PersonagemFake(int id, Time time)
        {
            this.id = id;
            this.time = time;
        }

        public Time Time
        {
            get { return time; }
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
    }
}