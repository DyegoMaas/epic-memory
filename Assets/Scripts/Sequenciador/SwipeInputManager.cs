using System.Collections.Generic;
using Assets.Scripts;
using EpicMemory.Sequenciador;
using UnityEngine;

public class SwipeInputManager : IInputManager
{
    private readonly RepositorioPersonagens repositorio;
    private readonly List<int> touchesProcessed = new List<int>();

    public SwipeInputManager(RepositorioPersonagens repositorio)
    {
        this.repositorio = repositorio;
    }

    private IPersonagem antigoPersonagem = null;

    public bool Click(out IPersonagem personagemSelecionado)
    {
        personagemSelecionado = null;

        Vector3 position;
        if (Touch(out position))
        {
            var novoPersonagem = Raycast(position);
            if (novoPersonagem != null)
            {
                if (antigoPersonagem != null)
                {
                    if (novoPersonagem != antigoPersonagem)
                    {
                        personagemSelecionado = novoPersonagem;
                        antigoPersonagem = novoPersonagem;
                    }
                }
                else
                {
                    personagemSelecionado = novoPersonagem;
                    antigoPersonagem = novoPersonagem;
                }
            }
            else
                antigoPersonagem = null;
        }
        else
            antigoPersonagem = null;

        return personagemSelecionado != null;
    }
    
    private bool Touch(out Vector3 position)
    {
        if (Input.GetButton("Fire1"))
        {
            position = Input.mousePosition;
            return true;
        }

        if (Input.touchCount == 1)
        {
            position = Input.GetTouch(0).position;
            return true;
        }

        position = Vector3.zero;
        return false;
    }

    private IPersonagem Raycast(Vector3 clickPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(clickPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            foreach (var personagem in repositorio.BuscarTodos())
            {
                //TODO: arrumar esse cast estranho
                if (PersonagemFoiSelecionado(hit, personagem as IPersonagemJogo))
                {
                    return personagem;
                }
            }
        }
        return null;
    }

    private bool PersonagemFoiSelecionado(RaycastHit hit, IPersonagemJogo personagem)
    {
        return personagem.GetCollider().transform == hit.transform;
    }
}