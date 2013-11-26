using NSubstitute;
using NUnit.Framework;

namespace SSaME.Core.Testes.Temas
{
    public class GuardiaoChavesTeste
    {
        public const string ChaveTemasInstalados = "Temas";
        public const string ChaveSelecionado = "TemaSelecionado";
        
        [Test]
        public void todos_os_temas_devem_estar_cadastrados()
        {
            var gerenciadorChaves = DadoUmGerenciadorChaves();
            var gerenciador = new GuardiaoChaves();
            string listaTemasInstalados = gerenciador.Obter(ChaveTemasInstalados);
            AListaDeveConterTodosOsValoresDosTemasInstalados();
        }

        private IGerenciadorChaves DadoUmGerenciadorChaves()
        {
            return null;
            //return Substitute.For<IGerenciadorChaves>().When(e => e.Atacar(Arg.));
        }

        private void AListaDeveConterTodosOsValoresDosTemasInstalados()
        {
            //IGerenciadorTemas temasInstalados = new TemasInstaladosStub();
            //temasInstalados.
        }
    }

    public interface IGerenciadorChaves
    {
    }

    public class GuardiaoChaves  
    {
        public string Obter(string chaveTemasInstalados)
        {
            return string.Empty;
        }
    }
}