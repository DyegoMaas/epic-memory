using System;
using System.Collections.Generic;
using System.Linq;
using SSaME.Core;

public class RepositorioPersonagens
{
    private static readonly Dictionary<int, IPersonagem> Personagens = new Dictionary<int, IPersonagem>();
    private static readonly Dictionary<int, IPersonagem> TimeA = new Dictionary<int, IPersonagem>();
    private static readonly Dictionary<int, IPersonagem> TimeB = new Dictionary<int, IPersonagem>();

    public void AdicionarPersonagem(IPersonagem personagem)
    {
        Personagens.Add(personagem.Id, personagem);

        var time = (personagem.Time == Times.TimeA) ? TimeA : TimeB;
        time.Add(personagem.Id, personagem);
    }

    public IList<IPersonagem> BuscarTodos()
    {
        return Personagens.Values.ToList();
    }

    public IPersonagem Buscar(int idPersonagem)
    {
        return BuscarPersonagem(idPersonagem, Personagens);
    }

    public IPersonagem BuscarNoTimeA(int idPersonagem)
    {
        return BuscarPersonagem(idPersonagem, TimeA);
    }

    public IPersonagem BuscarNoTimeB(int idPersonagem)
    {
        return BuscarPersonagem(idPersonagem, TimeB);
    }

    private IPersonagem BuscarPersonagem(int idPersonagem, Dictionary<int, IPersonagem> repositorio)
    {
        if(!repositorio.ContainsKey(idPersonagem))
        {
            throw new Exception("Personagem não encontrado: " + idPersonagem);
        }

        return repositorio[idPersonagem];
    }
}