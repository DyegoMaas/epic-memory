using UnityEngine;
using System.Collections;

public class Teste : InjectionBehaviour {

    public ITeste TesteX;

    protected override void StartOverride()
    {
        Debug.Log("injecao");
        TesteX.Testar();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
