using EpicMemory.Sequenciador;

namespace SSaME.Core.Sequenciador
{
    public interface IPersonagem
    {
        Equipe Equipe { get; }
        int Id { get; }
        int Nivel { get; }
        int Vida { get; }

        void Inicializar(int id);
        void Selecionar();
        void Atacar();
        void SubirNivel();
        void ResetarNivel();
        void AdicionarVida(int vida);
    }
}