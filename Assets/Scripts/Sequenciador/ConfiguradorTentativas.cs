using UnityEngine;
using System.Collections;

public class ConfiguradorTentativas : InjectionBehaviour {

    public int NumeroTentativas = 3;

    [InjectedDependency] private IContadorTentativas contadorTentativas;

	// Use this for initialization
	protected override void StartOverride () {
        contadorTentativas.ConfigurarNumeroTentativas(NumeroTentativas);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
