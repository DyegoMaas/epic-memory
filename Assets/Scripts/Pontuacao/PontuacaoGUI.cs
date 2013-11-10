using UnityEngine;
using System.Collections;

public class PontuacaoGUI : MonoBehaviour {

    private tk2dTextMesh textMesh;

	// Use this for initialization
	void Start () {
        textMesh = GetComponent<tk2dTextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void AtualizarPontuacao(int pontos)
    {
        textMesh.text = pontos + " x";
    }
}
