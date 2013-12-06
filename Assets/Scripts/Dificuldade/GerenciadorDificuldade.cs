using UnityEngine;
using System.Collections;

public class GerenciadorDificuldade : MonoBehaviour {

    [SerializeField]
    private Dificuldade dificuldade = Dificuldade.Facil;

    public Dificuldade Dificuldade
    {
        get { return dificuldade; }
    }
    
    /// <summary>
    /// 0 == impossível
    /// 1 == fácil
    /// </summary>
    public float CoeficienteFacilidade
    {
        get
        {
            if (Dificuldade == Dificuldade.Facil)
                return 1f;
            return .5f;
        }
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
        Debug.Log(dificuldade.ToString());
    }
}