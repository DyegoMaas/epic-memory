using SSaME.Core.Sequenciador;

namespace SSaME.Core.Testes.Sequenciador
{
    public class PersonagemTesteBuilder
    {
        private int idPersonagem;
        private Equipe equipe = Equipe.A;

        public PersonagemTesteBuilder ComId(int idPersonagem)
        {
            this.idPersonagem = idPersonagem;
            return this;
        }

        public PersonagemTesteBuilder DoTime(Equipe equipe)
        {
            this.equipe = equipe;
            return this;
        }

        public IPersonagem Construir()
        {
            return new PersonagemFake(idPersonagem, equipe);
        }
    }
}