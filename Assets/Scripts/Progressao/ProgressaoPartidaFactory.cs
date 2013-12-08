using System;
using UnityEngine;

namespace Assets.Scripts.Progressao
{
    public class ProgressaoPartidaFactory : InjectionBehaviour, IProgressaoPartidaFactory
    {
        public TipoProgressaoPartida TipoProgressaoPartida = TipoProgressaoPartida.Linear;
        public int numeroPartidasSubirNivel = 2;

        [InjectedDependency]
        private RepositorioPersonagens repositorio;

        protected override void StartOverride()
        {
            
        }

        // Update is called once per frame
        void Update () {
	
        }

        public IProgressaoPartida CriarProgressorPartida()
        {
            switch (TipoProgressaoPartida)
            {
                case TipoProgressaoPartida.Linear: return new ProgressaoLinear(repositorio, numeroPartidasSubirNivel);
                case TipoProgressaoPartida.Assimetrica: return new ProgressaoAssimetrica(repositorio);
            }
            throw new InvalidOperationException("Não existe progressor de partida do tipo " + TipoProgressaoPartida);
        }
    }

    public interface IProgressaoPartidaFactory
    {
        IProgressaoPartida CriarProgressorPartida();
    }

    //public class ProgressaoPartidaFactory : IProgressaoPartidaFactory
    //{
    //    private readonly RepositorioPersonagens repositorio;
    //    private ProgressaoLinear progressaoLinear;
    //    private TipoProgressaoPartida tipoProgressaoSelecionado;
    //    private ProgressaoAssimetrica progressaoAssimetrica;

    //    public void ConfigurarProgressaoLinear(int numeroPartidasParaSubirNivel)
    //    {
    //        progressaoLinear = new ProgressaoLinear(repositorio, numeroPartidasParaSubirNivel);
    //        tipoProgressaoSelecionado = TipoProgressaoPartida.Linear;
    //    }

    //    public void ConfigurarProgressaoAssimetrica()
    //    {
    //        progressaoAssimetrica = new ProgressaoAssimetrica(repositorio);
    //        tipoProgressaoSelecionado = TipoProgressaoPartida.Assimetrica;
    //    }

    //    public IProgressaoPartida CriarProgressorPartida(RepositorioPersonagens repositorio)
    //    {
    //        switch (tipoProgressaoSelecionado)
    //        {
    //            case TipoProgressaoPartida.Linear: return progressaoLinear;
    //            case TipoProgressaoPartida.Assimetrica: return progressaoAssimetrica;
    //        }
    //        throw new InvalidOperationException("Não existe progressor de partida do tipo " + tipoProgressaoSelecionado);
    //    }
    //}
}