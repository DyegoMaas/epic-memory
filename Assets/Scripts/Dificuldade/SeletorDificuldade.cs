using UnityEngine;
using System.Collections;

public class SeletorDificuldade : InjectionBehaviour {

    public Dificuldade dificuldadeInicial = Dificuldade.Facil;

    [InjectedDependency]
    private GerenciadorDificuldade gerenciadorDificuldade;

    protected override void StartOverride()
    {
        gerenciadorDificuldade.EscolherDificuldade(dificuldadeInicial);
    }

    // Update is called once per frame
	void Update () {
	
	}

    public void EscolherDificuldade(Dificuldade novaDificuldade)
    {
        gerenciadorDificuldade.EscolherDificuldade(novaDificuldade);
    }
}