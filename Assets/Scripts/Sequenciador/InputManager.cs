using System;
using System.Collections.Generic;
using Assets.Scripts;
using EpicMemory.Sequenciador;
using UnityEngine;

public class InputManager : IInputManager
{
    private readonly RepositorioPersonagens repositorio;
    private readonly List<int> touchesProcessed = new List<int>();

    public InputManager(RepositorioPersonagens repositorio)
    {
        this.repositorio = repositorio;
    }

    public bool Click(out IPersonagem personagemSelecionado)
    {
        personagemSelecionado = null;

        if (CliqueMouse())
        {
            personagemSelecionado = Raycast(Input.mousePosition);
        }
        else if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (!touchesProcessed.Contains(touch.fingerId))
            {
                personagemSelecionado = Raycast(touch.position);
                touchesProcessed.Add(touch.fingerId);
            }
        }

        return personagemSelecionado != null;
    }

    public bool Click(out IPersonagem personagemSelecionado, out Vector3 posicao)
    {
        throw new NotImplementedException();
    }

    private static bool CliqueMouse()
    {
        return Input.GetButtonDown("Fire1") || Input.GetButtonUp("Fire1");
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