using FluentAssertions;
using NUnit.Framework;

namespace SSaME.Core.Testes
{
    [TestFixture]
    public class ValidadorAtaquesTeste
    {
        [Test]
        [TestCase(Time.A, Time.B, true)]
        [TestCase(Time.A, Time.A, false)]
        [TestCase(Time.B, Time.A, true)]
        [TestCase(Time.B, Time.B, false)]
        public void o_atacante_deve_atacar_um_alvo_do_outro_time(Time timeAtacante, Time timeAlvo, bool ataqueValido)
        {
            var atacante = new PersonagemFake(0, timeAtacante);
            var alvo = new PersonagemFake(0, timeAlvo);

            var ataque = new Ataque(atacante, alvo);
            var validadorAtaque = new ValidadorAtaques();

            validadorAtaque.AtaqueValido(ataque).Should().Be(ataqueValido);
        }
    }
}