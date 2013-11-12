using UnityEngine;
using System.Collections;

public class PontuacaoSom : MonoBehaviour
{

    public AudioClip somMoeda;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void AtualizarPontuacao()
    {
        if(somMoeda)
            AudioSource.PlayClipAtPoint(somMoeda, transform.position);
    }
}
