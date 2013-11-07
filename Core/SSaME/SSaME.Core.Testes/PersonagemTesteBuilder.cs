namespace SSaME.Core.Testes
{
    public class PersonagemTesteBuilder
    {
        private int idPersonagem;
        private Times time = Times.TimeA;

        public PersonagemTesteBuilder ComId(int idPersonagem)
        {
            this.idPersonagem = idPersonagem;
            return this;
        }

        public PersonagemTesteBuilder DoTime(Times time)
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