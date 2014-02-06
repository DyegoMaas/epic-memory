using EpicMemory.Sequenciador;
using UnityEngine;

public interface IInputManager
{
    bool Click(out IPersonagem personagemSelecionado);
    bool Click(out IPersonagem personagemSelecionado, out Vector3 posicao);
}