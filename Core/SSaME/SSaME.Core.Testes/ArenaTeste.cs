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
            var arena = DadoUmaArenaVazia();

            var idParticipante = arena.AdicionarParticipanteAoTimeA();

            arena.TimeA.Should().NotBeNull();
            arena.TimeA.Count.Should().Be(1);
            arena.TimeA[0].Should().Be(idParticipante);
        }

        [Test]
        public void eh_possivel_adicionar_participantes_no_time_b()
        {
            var arena = DadoUmaArenaVazia();

            var idParticipante = arena.AdicionarParticipanteAoTimeB();

            arena.TimeB.Should().NotBeNull();
            arena.TimeB.Count.Should().Be(1);
            arena.TimeB[0].Should().Be(idParticipante);
        }

        [Test]
        public void os_ids_sao_sequenciais()
        {
            var arena = DadoUmaArenaVazia();

            var idParticipanteA = arena.AdicionarParticipanteAoTimeA();
            var idParticipanteB = arena.AdicionarParticipanteAoTimeA();
            var idParticipanteC = arena.AdicionarParticipanteAoTimeB();
            var idParticipanteD = arena.AdicionarParticipanteAoTimeB();

            idParticipanteD.Should().BeGreaterThan(idParticipanteC);
            idParticipanteC.Should().BeGreaterThan(idParticipanteB);
            idParticipanteB.Should().BeGreaterThan(idParticipanteA);
        }

        private Arena DadoUmaArenaVazia()
        {
            return new Arena();
        }
    }
}