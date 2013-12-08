using EpicMemory.Sequenciador;
using UnityEngine;

public class UnityRandomizer : IRandom {

    public bool Bool()
    {
        return Random.value < .5f; 
    }

    public int Range(int valorInicial, int valorFinal)
    {
        return Random.Range(valorInicial, valorFinal);
    }
}
