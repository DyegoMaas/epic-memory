using UnityEngine;
using System.Collections;

public class Personagem : MonoBehaviour {

    public Times Time;
    public int Id { get; private set; }
    
    private tk2dSpriteAnimator spriteAnimator;
    
    // Use this for initialization
	void Start ()
	{
	    spriteAnimator = GetComponent<tk2dSpriteAnimator>();
	}

    // Update is called once per frame
	void Update () {
	
	}

    public void Inicializar(int id, Times time)
    {
        Id = id;
        gameObject.name += "_" + id.ToString().PadLeft(2, '0');

        Time = time;
    }

    public void Selecionar()
    {
        Debug.Log("selecionado " + gameObject.name);
        StartCoroutine(AnimarSelecao());
    }

    private IEnumerator AnimarSelecao()
    {

        iTween.ScaleTo(gameObject, new Vector3(1.5f, 1.5f, 1), .2f);
        yield return new WaitForSeconds(.1f);
        iTween.ScaleTo(gameObject, new Vector3(1, 1, 1), .2f);
    }

    public void Atacar()
    {
        spriteAnimator.Play("Ataque");
    }
}
