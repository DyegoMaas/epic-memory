using FluentAssertions;
using NUnit.Framework;

namespace SSaME.Core.Testes
{
    [TestFixture]
    public class ArenaTeste
    {
        [Test]
        public void eh_possivel_adicionar_participantes_no_time_a()
        {
            var personagem = new PersonagemTesteBuilder()
                .DoTime(Times.TimeA)
                .Construir();
            var arena = DadoUmaArenaVazia();

            arena.AdicionarParticipante(personagem);

            arena.TimeA.Should().NotBeNull();
            arena.TimeA.Count.Should().Be(1);
            arena.TimeA[0].Id.Should().Be(1);
        }

        [Test]
        public void eh_possivel_adicionar_participantes_no_time_b()
        {
            var personagem = new PersonagemTesteBuilder()
                .DoTime(Times.TimeB)
                .Construir();
            var arena = DadoUmaArenaVazia();

            arena.AdicionarParticipante(personagem);

            arena.TimeB.Should().NotBeNull();
            arena.TimeB.Count.Should().Be(1);
            arena.TimeB[0].Id.Should().Be(1);
        }

        [Test]
        public void os_ids_sao_sequenciais()
        {
            var participanteA = new PersonagemTesteBuilder()
                .DoTime(Times.TimeA)
                .Construir();
            var participanteB = new PersonagemTesteBuilder()
                .DoTime(Times.TimeA)
                .Construir();
            var participanteC = new PersonagemTesteBuilder()
                .DoTime(Times.TimeB)
                .Construir();
            var participanteD = new PersonagemTesteBuilder()
                .DoTime(Times.TimeB)
                .Construir();

            var arena = DadoUmaArenaVazia();

            arena.AdicionarParticipante(participanteA);
            arena.AdicionarParticipante(participanteB);
            arena.AdicionarParticipante(participanteC);
            arena.AdicionarParticipante(participanteD);

            participanteD.Id.Should().BeGreaterThan(participanteC.Id);
            participanteC.Id.Should().BeGreaterThan(participanteB.Id);
            participanteB.Id.Should().BeGreaterThan(participanteA.Id);
        }

        private Arena DadoUmaArenaVazia()
        {
            return new Arena();
        }
    }
}