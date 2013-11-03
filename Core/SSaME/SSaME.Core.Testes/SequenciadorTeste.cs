using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace SSaME.Core.Testes
{
    [TestFixture]
    public class SequenciadorTeste
    {
        private const int IdJogadorATimeA = 1;
        private const int IdJogadorBTimeA = 2;
        private const int IdJogadorATimeB = 3;
        private const int IdJogadorBTimeB = 4;

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

            arena.TimeA.Returns(new List<int> {IdJogadorATimeA, IdJogadorBTimeA});
            arena.TimeB.Returns(new List<int> {IdJogadorATimeB, IdJogadorBTimeB});

            return arena;
        }

        private IRandom DadoUmRandomizador()
        {
            var random = Substitute.For<IRandom>();

            random.Bool().Returns(true);
            random.NextInt(0, 1).Returns(0);

            return random;
        }

        private Sequenciador DadoUmSequenciador(IArena arena, IRandom random)
        {
            return new Sequenciador(arena, random);
        }

        private void AlvoEAtacanteDevemEstarEntreOsJogadoresDaArena(Ataque ataque)
        {
            var listaJogadores = new List<int> {IdJogadorATimeA, IdJogadorATimeB, IdJogadorBTimeA, IdJogadorBTimeB};
            listaJogadores.Should().Contain(ataque.Atacante);
            listaJogadores.Should().Contain(ataque.Alvo);
        }

        private void AlvoEAtavanteNaoDevemEstarNoMesmoTime(IArena arena, Ataque ataque)
        {
            if (ataque.TimeAtacante == TimeAtacante.TimeA)
                arena.TimeA.Should().NotContain(ataque.Alvo);
            else
                arena.TimeA.Should().NotContain(ataque.Atacante);
        }
    }
}
