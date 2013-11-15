namespace SSaME.Core
{
    public interface IPersonagem
    {
        Equipe Equipe { get; }
        int Id { get; }
        int Nivel { get; }

        void Inicializar(int id);
        void Selecionar();
        void Atacar();
        void SubirNivel();
        void ResetarNivel();
    }
}