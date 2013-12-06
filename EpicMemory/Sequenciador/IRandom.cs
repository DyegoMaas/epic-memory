namespace SSaME.Core.Sequenciador
{
    public interface IRandom  
    {
        bool Bool();
        int Range(int valorMinimo, int valorMaximo);
    }
}