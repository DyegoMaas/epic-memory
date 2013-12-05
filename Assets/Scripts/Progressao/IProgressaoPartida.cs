using AllLogic.Sequenciador;
using SSaME.Core.Sequenciador;
using UnityEngine;
using System.Collections;

public interface IProgressaoPartida
{
    void AtualizarProgressao(Ataque ataque);
    void ResetarProgressoPartida();
}