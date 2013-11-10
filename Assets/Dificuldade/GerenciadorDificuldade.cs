using UnityEngine;
using System.Collections;

public class GerenciadorDificuldade : MonoBehaviour {

    [SerializeField]
    private Dificuldade dificuldade = Dificuldade.Facil;

    public Dificuldade Dificuldade
    {
        get { return dificuldade; }
    }
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void EscolherDificuldade(Dificuldade novaDificuldade)
    {
        dificuldade = novaDificuldade;
    }
}