using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace SSaME.Core.Testes
{
    [TestFixture]
    public class GeradorAtaquesTeste
    {
        private readonly IPersonagem jogadorATimeA = new PersonagemFake(1, Equipe.A);
        private readonly IPersonagem jogadorBTimeA = new PersonagemFake(2, Equipe.A);
        private readonly IPersonagem jogadorATimeB = new PersonagemFake(3, Equipe.B);
        private readonly IPersonagem jogadorBTimeB = new PersonagemFake(4, Equipe.B);

        [Test]
        public void sequenciador_gera_um_ataque()
        {
            var arena = DadaUmaArenaComDoisJogadoresEmCadaTime();
            var random = DadoUmRandomizador();
            var sequenciador = DadoUmSequenciador(arena, random);

            Ataque ataque = sequenciador.GerarAtaque();

            AlvoEAtacanteDevemEstarEntreOsJogadoresDaArena(ataque);
            AlvoEAtavanteNaoDevemEstarNoMesmoTime(arena, ataque);
        }

        private IArena DadaUmaArenaComDoisJogadoresEmCadaTime()
        {
            var arena = Substitute.For<IArena>();

            arena.TimeA.Returns(new List<IPersonagem> {jogadorATimeA, jogadorBTimeA});
            arena.TimeB.Returns(new List<IPersonagem> { jogadorATimeB, jogadorBTimeB });

            return arena;
        }

        private IRandom DadoUmRandomizador()
        {
            var random = Substitute.For<IRandom>();

            random.Bool().Returns(true);
            random.Range(0, 1).Returns(0);

            return random;
        }

        private GeradorAtaques DadoUmSequenciador(IArena arena, IRandom random)
        {
            return new GeradorAtaques(arena, random);
        }

        private void AlvoEAtacanteDevemEstarEntreOsJogadoresDaArena(Ataque ataque)
        {
            var listaJogadores = new List<IPersonagem> {jogadorATimeA, jogadorATimeB, jogadorBTimeA, jogadorBTimeB};
            listaJogadores.Should().Contain(ataque.Atacante);
            listaJogadores.Should().Contain(ataque.Alvo);
        }

        private void AlvoEAtavanteNaoDevemEstarNoMesmoTime(IArena arena, Ataque ataque)
        {
            if (ataque.Atacante.Equipe == Equipe.A)
                arena.TimeA.Should().NotContain(ataque.Alvo);
            else
                arena.TimeA.Should().NotContain(ataque.Atacante);
        }
    }
}
