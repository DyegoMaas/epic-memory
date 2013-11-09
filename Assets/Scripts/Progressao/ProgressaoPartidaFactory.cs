using UnityEngine;

namespace Assets.Scripts.Progressao
{
    public interface IProgressaoPartidaFactory
    {
        IProgressaoPartida CriarProgressorPartida(RepositorioPersonagens repositorio);
    }

    public class ProgressaoPartidaFactory : MonoBehaviour, IProgressaoPartidaFactory
    {

        public TipoProgressaoPartida TipoProgressaoPartida = TipoProgressaoPartida.Linear;
        public int numeroPartidasSubirNivel = 2;

        // Use this for initialization
        void Start () {
	
        }
	
        // Update is called once per frame
        void Update () {
	
        }

        public IProgressaoPartida CriarProgressorPartida(RepositorioPersonagens repositorio)
        {
            switch (TipoProgressaoPartida)
            {
                case TipoProgressaoPartida.Linear: return new ProgressaoLinear(repositorio, numeroPartidasSubirNivel);
                case TipoProgressaoPartida.Assimetrica: return new ProgressaoAssimetrica(repositorio);
            }
            throw new UnityException("Não existe progressor de partida do tipo " + TipoProgressaoPartida);
        }
    }
}