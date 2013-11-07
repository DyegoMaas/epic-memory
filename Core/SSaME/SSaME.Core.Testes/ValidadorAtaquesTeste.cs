using FluentAssertions;
using NUnit.Framework;

namespace SSaME.Core.Testes
{
    [TestFixture]
    public class ValidadorAtaquesTeste
    {
        [Test]
        [TestCase(Times.TimeA, Times.TimeB, true)]
        [TestCase(Times.TimeA, Times.TimeA, false)]
        [TestCase(Times.TimeB, Times.TimeA, true)]
        [TestCase(Times.TimeB, Times.TimeB, false)]
        public void o_atacante_deve_atacar_um_alvo_do_outro_time(Times timeAtacante, Times timeAlvo, bool ataqueValido)
        {
            var atacante = new PersonagemFake(0, timeAtacante);
            var alvo = new PersonagemFake(0, timeAlvo);

            var ataque = new Ataque(atacante, alvo);
            var validadorAtaque = new ValidadorAtaques();

            validadorAtaque.AtaqueValido(ataque).Should().Be(ataqueValido);
        }
    }
}