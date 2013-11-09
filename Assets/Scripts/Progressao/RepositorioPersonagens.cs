using System.Collections.Generic;
using System.Linq;
using SSaME.Core;

public class RepositorioPersonagens : IArena
{
    private readonly Dictionary<int, IPersonagem> Personagens = new Dictionary<int, IPersonagem>();
    public List<IPersonagem> TimeA { get; private set; }
    public List<IPersonagem> TimeB { get; private set; }
    
    private static int idAtual;

    public RepositorioPersonagens()
    {
        TimeA = new List<IPersonagem>();
        TimeB = new List<IPersonagem>();
    }

    public void Adicionar(IPersonagem personagem)
    {
        personagem.Inicializar(ProximoId());
        Personagens.Add(personagem.Id, personagem);

        var time = (personagem.Equipe == Equipe.A) ? TimeA : TimeB;
        time.Add(personagem);
    }

    public List<IPersonagem> BuscarTodos()
    {
        return Personagens.Values.ToList();
    }

    private static int ProximoId()
    {
        return ++idAtual;
    }
}