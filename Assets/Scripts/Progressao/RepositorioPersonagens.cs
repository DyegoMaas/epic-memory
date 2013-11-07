using System;
using System.Collections.Generic;
using System.Linq;
using SSaME.Core;

public class RepositorioPersonagens
{
    private static readonly Dictionary<int, Personagem> Personagens = new Dictionary<int, Personagem>();
    private static readonly Dictionary<int, Personagem> TimeA = new Dictionary<int, Personagem>();
    private static readonly Dictionary<int, Personagem> TimeB = new Dictionary<int, Personagem>();

    public void Adicionar(Personagem personagem)
    {
        Personagens.Add(personagem.Id, personagem);

        var time = (personagem.Time == Times.TimeA) ? TimeA : TimeB;
        time.Add(personagem.Id, personagem);
    }

    public IList<Personagem> BuscarTodos()
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

    private IPersonagem BuscarPersonagem(int idPersonagem, Dictionary<int, Personagem> repositorio)
    {
        if(!repositorio.ContainsKey(idPersonagem))
        {
            throw new Exception("Personagem não encontrado: " + idPersonagem);
        }

        return repositorio[idPersonagem];
    }
}