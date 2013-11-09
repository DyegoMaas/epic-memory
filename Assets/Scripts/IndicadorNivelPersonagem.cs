using UnityEngine;
using System.Collections;

[RequireComponent(typeof(tk2dTextMesh))]
public class IndicadorNivelPersonagem : MonoBehaviour {

    private int nivel;
    private tk2dTextMesh texto;

	// Use this for initialization
	void Start () {
        texto = GetComponent<tk2dTextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
   
    public void AtualizarNivelPersonagem(int novoNivel)
    {
        nivel = novoNivel;

        AtualizarTextMesh();
    }

    private void AtualizarTextMesh()
    {
        texto.text = "Nivel: " + nivel;
        texto.Commit();
    }
}
