using System;
using FluentAssertions;
using NUnit.Framework;
using SSaME.Core.Sequenciador;

namespace SSaME.Core.Testes.Sequenciador
{
    [TestFixture]
    public class ValidadorAtaquesTeste
    {
        [Test]
        [TestCase(Equipe.A, Equipe.B, true)]
        [TestCase(Equipe.A, Equipe.A, false)]
        [TestCase(Equipe.B, Equipe.A, true)]
        [TestCase(Equipe.B, Equipe.B, false)]
        public void o_atacante_deve_atacar_um_alvo_do_outro_time(Equipe equipeAtacante, Equipe equipeAlvo, bool ataqueValido)
        {
            var atacante = new PersonagemFake(0, equipeAtacante);
            var alvo = new PersonagemFake(0, equipeAlvo);

            var ataque = new Ataque(atacante, alvo);
            var validadorAtaque = new ValidadorAtaques();

            validadorAtaque.AtaqueValido(ataque).Should().Be(ataqueValido);
        }
    }
}