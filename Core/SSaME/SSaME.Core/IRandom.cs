namespace SSaME.Core
{
    public interface IRandom  
    {
        bool Bool();
        int NextInt(int valorInicial, int valorFinal);
    }
}