using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace SSaME.Core.Testes
{
    [TestFixture]
    public class ValidadorAtaquesTeste
    {
        private const int IdAtacanteTimeA = 1;
        private const int IdAlvoTimeA = 2;
        private const int IdAtacanteTimeB = 3;
        private const int IdAlvoTimeB = 4;

        [Test]
        public void um_personagem_pode_atacar_outro_do_time_oponente()
        {
            var arena = DadoUmaArenaComDoisJogadoresDeCadaLado();
            var ataque = DadoUmAtaqueDeUmGuerreiroAOutroDoTimeOponente();

            OAtaqueDeveSerConsideradoValido(arena, ataque);
        }

        [Test]
        public void um_personagem_nao_pode_atacar_outro_do_mesmo_time()
        {
            var arena = DadoUmaArenaComDoisJogadoresDeCadaLado();
            var ataque = DadoUmAtaqueDeUmGuerreiroAOutroDoMesmoTime();

            OAtaqueDeveSerConsideradoInvalido(arena, ataque);
        }

        [Test]
        public void o_time_atacante_deve_ser_o_mesmo_do_time_do_atacante()
        {
            var arena = DadoUmaArenaComDoisJogadoresDeCadaLado();
            var ataque = DadoUmAtaqueDoTimeAAoTimeB();

            OJogadorAtacanteDevePertencerAoTimeAtacante(arena, ataque);
            OAtaqueDeveSerConsideradoValido(arena, ataque);
        }

        [Test]
        public void o_time_atacante_nao_pode_ser_diferente_do_time_do_atacante()
        {
            var arena = DadoUmaArenaComDoisJogadoresDeCadaLado();
            var ataque = DadoUmAtaqueComTimeDiferenteDoAtacante();

            OAtaqueDeveSerConsideradoInvalido(arena, ataque);
        }

        private IArena DadoUmaArenaComDoisJogadoresDeCadaLado()
        {
            var arena = Substitute.For<IArena>();
            arena.TimeA.Returns(new List<int> {IdAtacanteTimeA, IdAlvoTimeA});
            arena.TimeB.Returns(new List<int> {IdAtacanteTimeB, IdAlvoTimeB});

            return arena;
        }

        private Ataque DadoUmAtaqueDeUmGuerreiroAOutroDoMesmoTime()
        {
            return new Ataque(IdAtacanteTimeA, IdAlvoTimeA, Times.TimeA);
        }

        private Ataque DadoUmAtaqueDeUmGuerreiroAOutroDoTimeOponente()
        {
            return new Ataque(IdAtacanteTimeB, IdAlvoTimeA, Times.TimeB);
        }

        private Ataque DadoUmAtaqueComTimeDiferenteDoAtacante()
        {
            return new Ataque(IdAtacanteTimeA, IdAlvoTimeB, Times.TimeB);
        }

        private Ataque DadoUmAtaqueDoTimeAAoTimeB()
        {
            return new Ataque(IdAtacanteTimeA, IdAlvoTimeB, Times.TimeA);
        }

        private void OAtaqueDeveSerConsideradoValido(IArena arena, Ataque ataque)
        {
            var validador = new ValidadorAtaques(arena);
            validador.AtaqueValido(ataque).Should().BeTrue();
        }

        private void OAtaqueDeveSerConsideradoInvalido(IArena arena, Ataque ataque)
        {
            var validador = new ValidadorAtaques(arena);
            validador.AtaqueValido(ataque).Should().BeFalse();
        }

        private void OJogadorAtacanteDevePertencerAoTimeAtacante(IArena arena, Ataque ataque)
        {
            if (ataque.TimeAtacante == Times.TimeA)
                arena.TimeA.Should().Contain(ataque.Atacante);
            else
                arena.TimeB.Should().Contain(ataque.Atacante);
        }
    }
}