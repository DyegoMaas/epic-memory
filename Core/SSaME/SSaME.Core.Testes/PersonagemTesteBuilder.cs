namespace SSaME.Core.Testes
{
    public class PersonagemTesteBuilder
    {
        private int idPersonagem;
        private Time time = Time.A;

        public PersonagemTesteBuilder ComId(int idPersonagem)
        {
            this.idPersonagem = idPersonagem;
            return this;
        }

        public PersonagemTesteBuilder DoTime(Time time)
        {
            this.time = time;
            return this;
        }

        public IPersonagem Construir()
        {
            return new PersonagemFake(idPersonagem, time);
        }
    }
}