namespace SSaME.Core
{
    public interface IPersonagem
    {
        Time Time { get; }
        int Id { get; }
        int Nivel { get; }

        void Inicializar(int id);
        void Selecionar();
        void Atacar();
        void SubirNivel();
    }
}