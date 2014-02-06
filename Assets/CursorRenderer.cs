using EpicMemory.Sequenciador;
using UnityEngine;
using System.Collections;

public class CursorRenderer : InjectionBehaviour
{

    [InjectedDependency]
    private IInputManager inputManager;
    
    protected override void StartOverride()
    {
        
    }

    // Update is called once per frame
	void Update ()
	{
	    IPersonagem personagem;
	    Vector3 posicao;
	    if (inputManager.Click(out personagem, out posicao))
        {
                        
        }
	}
}
