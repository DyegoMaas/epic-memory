using System.Collections.Generic;
using EpicMemory.Sequenciador;
using FluentAssertions;
using NUnit.Framework;

namespace EpicMemory.Testes.Unidade.Sequenciador
{
    [TestFixture]
    public class SequenciaAtaqueTeste
    {
        private readonly IPersonagem jogadorATimeA = new PersonagemFake(1, Equipe.A);
        private readonly IPersonagem jogadorBTimeA = new PersonagemFake(2, Equipe.A);
        private readonly IPersonagem jogadorATimeB = new PersonagemFake(3, Equipe.A);

        [Test]
        public void o_usuario_reproduz_um_ataque_corretamente()
        {
            var sequencia = DadoUmaSequenciaVazia();
            var ataque = DadoUmAtaqueDoTimeA();
            var listaAtaquesGravados = DadaUmaSequenciaDeAtaquesComEstesAtaques(ataque);

            QuandoOUsuarioReproduzirOsAtaques(sequencia, ataque);

            ASequenciaDeveEstarValida(listaAtaquesGravados, sequencia);
        }

        [Test]
        public void o_usuario_cria_um_ataque_proveniente_do_time_errado()
        {
            var sequencia = DadoUmaSequenciaVazia();
            var ataqueGravado = DadoUmAtaqueDoTimeA();
            var ataqueReproduzido = DadoUmAtaqueDoTimeB();
            var listaAtaquesGravados = DadaUmaSequenciaDeAtaquesComEstesAtaques(ataqueGravado);

            QuandoOUsuarioReproduzirOsAtaques(sequencia, ataqueReproduzido);

            ASequenciaDeveEstarInvalida(listaAtaquesGravados, sequencia);
        }

        [Test]
        public void o_usuario_reproduz_dois_ataques_corretamente()
        {
            var sequencia = DadoUmaSequenciaVazia();
            var ataque1 = DadoUmAtaqueDoTimeA();
            var ataque2 = DadoUmSegundoAtaqueDoTimeA();
            var listaAtaquesGravados = DadaUmaSequenciaDeAtaquesComEstesAtaques(ataque1, ataque2);

            QuandoOUsuarioReproduzirOsAtaques(sequencia, ataque1, ataque2);

            ASequenciaDeveEstarValida(listaAtaquesGravados, sequencia);
        }

        [Test]
        public void o_usuario_cria_um_ataque_proveniente_do_time_certo_mas_do_jogador_errado()
        {
            var sequencia = DadoUmaSequenciaVazia();
            var ataqueGravado1 = DadoUmAtaqueDoTimeA();
            var ataqueGravado2 = DadoUmSegundoAtaqueDoTimeA();
            var listaAtaquesGravados = DadaUmaSequenciaDeAtaquesComEstesAtaques(ataqueGravado1, ataqueGravado2);

            QuandoOUsuarioReproduzirOsAtaques(sequencia, ataqueGravado2, ataqueGravado2);

            ASequenciaDeveEstarInvalida(listaAtaquesGravados, sequencia);
        }

        [Test]
        public void eh_possivel_validar_uma_reproducao_parcial()
        {
            var sequencia = DadoUmaSequenciaVazia();
            var ataqueGravado1 = DadoUmAtaqueDoTimeA();
            var ataqueGravado2 = DadoUmSegundoAtaqueDoTimeA();
            var listaAtaquesGravados = DadaUmaSequenciaDeAtaquesComEstesAtaques(ataqueGravado1, ataqueGravado2);

            QuandoOUsuarioReproduzirOsAtaques(sequencia, ataqueGravado1);

            ASequenciaDeveEstarValida(listaAtaquesGravados, sequencia);
        }

        [Test]
        public void comparacao_de_duas_sequencias_de_ataque_de_mesmo_tamanho()
        {
            var ataqueGravado1 = DadoUmAtaqueDoTimeA();
            var ataqueGravado2 = DadoUmSegundoAtaqueDoTimeA();
            var sequenciaAtaque1 = DadaUmaSequenciaDeAtaquesComEstesAtaques(ataqueGravado1, ataqueGravado2);
            var sequenciaAtaque2 = DadaUmaSequenciaDeAtaquesComEstesAtaques(ataqueGravado1, ataqueGravado2);

            AsSequenciasDevemTerOMesmoTamanho(sequenciaAtaque1, sequenciaAtaque2);
        }

        [Test]
        public void comparacao_de_duas_sequencias_de_ataque_de_tamanhos_diferentes()
        {
            var ataqueGravado1 = DadoUmAtaqueDoTimeA();
            var ataqueGravado2 = DadoUmSegundoAtaqueDoTimeA();
            var sequenciaAtaque1 = DadaUmaSequenciaDeAtaquesComEstesAtaques(ataqueGravado1, ataqueGravado2);
            var sequenciaAtaque2 = DadaUmaSequenciaDeAtaquesComEstesAtaques(ataqueGravado1);

            ASegundaSequenciaNaoDeveEstarCompleta(sequenciaAtaque1, sequenciaAtaque2);
        }

        [Test]
        public void ao_converter_para_lista()
        {
            var ataqueGravado1 = DadoUmAtaqueDoTimeA();
            var ataqueGravado2 = DadoUmSegundoAtaqueDoTimeA();
            var sequenciaAtaque = DadaUmaSequenciaDeAtaquesComEstesAtaques(ataqueGravado1, ataqueGravado2);

            var listaAtaques = AoConverterParaLista(sequenciaAtaque);
            AListaDeveConterOsAtaquesDaSequencia(listaAtaques, ataqueGravado1, ataqueGravado2);
        }

        private static SequenciaAtaque DadoUmaSequenciaVazia()
        {
            return new SequenciaAtaque();
        }

        private Ataque DadoUmAtaqueDoTimeA()
        {
            return new Ataque(jogadorATimeA, jogadorATimeB);
        }

        private Ataque DadoUmSegundoAtaqueDoTimeA()
        {
            return new Ataque(jogadorBTimeA, jogadorATimeB);
        }

        private Ataque DadoUmAtaqueDoTimeB()
        {
            return new Ataque(jogadorATimeB, jogadorATimeA);
        }

        private static SequenciaAtaque DadaUmaSequenciaDeAtaquesComEstesAtaques(params Ataque[] ataques)
        {
            var sequenciaAtaque = new SequenciaAtaque();
            foreach (var ataque in ataques)
            {
                sequenciaAtaque.ArmazenarAtaque(ataque);
            }
            return sequenciaAtaque;
        }

        private static void QuandoOUsuarioReproduzirOsAtaques(SequenciaAtaque sequenciaAtaque, params Ataque[] ataques)
        {
            foreach (var ataque in ataques)
            {
                sequenciaAtaque.ArmazenarAtaque(ataque);
            }
        }

        private IList<Ataque> AoConverterParaLista(SequenciaAtaque sequenciaAtaque)
        {
            return sequenciaAtaque.ToList();
        }

        private static void ASequenciaDeveEstarValida(SequenciaAtaque sequenciaValidar, SequenciaAtaque sequenciaValida)
        {
            sequenciaValida.Validar(sequenciaValidar).Should().BeTrue();
        }

        private static void ASequenciaDeveEstarInvalida(SequenciaAtaque sequenciaValidar, SequenciaAtaque sequenciaValida)
        {
            sequenciaValida.Validar(sequenciaValidar).Should().BeFalse();
        }

        private void AsSequenciasDevemTerOMesmoTamanho(SequenciaAtaque sequenciaAtaque1, SequenciaAtaque sequenciaAtaque2)
        {
            sequenciaAtaque1.EstaCompleta(sequenciaAtaque2).Should().BeTrue();
        }

        private void ASegundaSequenciaNaoDeveEstarCompleta(SequenciaAtaque sequenciaAtaque1, SequenciaAtaque sequenciaAtaque2)
        {
            sequenciaAtaque1.EstaCompleta(sequenciaAtaque2).Should().BeFalse();
        }

        private void AListaDeveConterOsAtaquesDaSequencia(IList<Ataque> listaAtaques, params Ataque[] ataques)
        {
            listaAtaques.Count.Should().Be(ataques.Length);

            foreach (var ataque in ataques)
            {
                listaAtaques.Should().Contain(ataque);
            }
        }
    }
}