using System.Collections.Generic;
using SSaME.Core;
using UnityEngine;

public class InputManager
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

        if (Input.GetButtonDown("Fire1"))
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

    private IPersonagem Raycast(Vector3 clickPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(clickPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            foreach (var personagem in repositorio.BuscarTodos())
            {
                if (PersonagemFoiSelecionado(hit, personagem))
                {
                    return personagem;
                }
            }
        }
        return null;
    }

    private bool PersonagemFoiSelecionado(RaycastHit hit, IPersonagem personagem)
    {
        return personagem.GetCollider().transform == hit.transform;
    }
}