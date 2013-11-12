using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace SSaME.Core.Testes
{
    [TestFixture]
    public class SequenciaTeste
    {
        private readonly IPersonagem jogadorATimeA = new PersonagemFake(1, Time.A);
        private readonly IPersonagem jogadorBTimeA = new PersonagemFake(2, Time.A);
        private readonly IPersonagem jogadorATimeB = new PersonagemFake(3, Time.A);

        [Test]
        public void o_usuario_reproduz_um_ataque_corretamente()
        {
            var sequencia = DadoUmaSequenciaVazia();
            var ataque = DadoUmAtaqueDoTimeA();
            var listaAtaquesGravados = DadaUmaListaDeAtaquesComEstesAtaques(ataque);

            QuandoOUsuarioReproduzirOsAtaques(sequencia, ataque);

            ASequenciaDeveEstarValida(listaAtaquesGravados, sequencia);
        }

        [Test]
        public void o_usuario_cria_um_ataque_proveniente_do_time_errado()
        {
            var sequencia = DadoUmaSequenciaVazia();
            var ataqueGravado = DadoUmAtaqueDoTimeA();
            var ataqueReproduzido = DadoUmAtaqueDoTimeB();
            var listaAtaquesGravados = DadaUmaListaDeAtaquesComEstesAtaques(ataqueGravado);

            QuandoOUsuarioReproduzirOsAtaques(sequencia, ataqueReproduzido);

            ASequenciaDeveEstarInvalida(listaAtaquesGravados, sequencia);
        }

        [Test]
        public void o_usuario_reproduz_dois_ataques_corretamente()
        {
            var sequencia = DadoUmaSequenciaVazia();
            var ataque1 = DadoUmAtaqueDoTimeA();
            var ataque2 = DadoUmSegundoAtaqueDoTimeA();
            var listaAtaquesGravados = DadaUmaListaDeAtaquesComEstesAtaques(ataque1, ataque2);

            QuandoOUsuarioReproduzirOsAtaques(sequencia, ataque1, ataque2);

            ASequenciaDeveEstarValida(listaAtaquesGravados, sequencia);
        }

        [Test]
        public void o_usuario_cria_um_ataque_proveniente_do_time_certo_mas_do_jogador_errado()
        {
            var sequencia = DadoUmaSequenciaVazia();
            var ataqueGravado1 = DadoUmAtaqueDoTimeA();
            var ataqueGravado2 = DadoUmSegundoAtaqueDoTimeA();
            var listaAtaquesGravados = DadaUmaListaDeAtaquesComEstesAtaques(ataqueGravado1, ataqueGravado2);

            QuandoOUsuarioReproduzirOsAtaques(sequencia, ataqueGravado2, ataqueGravado2);

            ASequenciaDeveEstarInvalida(listaAtaquesGravados, sequencia);
        }

        [Test]
        public void eh_possivel_validar_uma_reproducao_parcial()
        {
            var sequencia = DadoUmaSequenciaVazia();
            var ataqueGravado1 = DadoUmAtaqueDoTimeA();
            var ataqueGravado2 = DadoUmSegundoAtaqueDoTimeA();
            var listaAtaquesGravados = DadaUmaListaDeAtaquesComEstesAtaques(ataqueGravado1, ataqueGravado2);

            QuandoOUsuarioReproduzirOsAtaques(sequencia, ataqueGravado1);

            ASequenciaDeveEstarValida(listaAtaquesGravados, sequencia);
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

        private static List<Ataque> DadaUmaListaDeAtaquesComEstesAtaques(params Ataque[] ataques)
        {
            return new List<Ataque>(ataques);
        }

        private static void QuandoOUsuarioReproduzirOsAtaques(SequenciaAtaque sequenciaAtaque, params Ataque[] ataques)
        {
            foreach (var ataque in ataques)
            {
                sequenciaAtaque.ArmazenarAtaque(ataque);
            }
        }

        private static void ASequenciaDeveEstarValida(List<Ataque> listaAtaques, SequenciaAtaque sequenciaAtaque)
        {
            sequenciaAtaque.Validar(listaAtaques).Should().BeTrue();
        }

        private static void ASequenciaDeveEstarInvalida(List<Ataque> listaAtaques, SequenciaAtaque sequenciaAtaque)
        {
            sequenciaAtaque.Validar(listaAtaques).Should().BeFalse();
        }
    }
}