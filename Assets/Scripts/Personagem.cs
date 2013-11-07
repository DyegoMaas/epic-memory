using SSaME.Core;
using UnityEngine;
using System.Collections;

public class Personagem : MonoBehaviour, IPersonagem {

    public Times Time { get; private set; }
    public int Id { get; private set; }
    public int Nivel { get; private set; }

    public Times TimePersonagem = Times.TimeA;

    private tk2dSpriteAnimator spriteAnimator;

    void Awake()
    {
        Time = TimePersonagem;
    }
    
    // Use this for initialization
	void Start ()
	{
	    spriteAnimator = GetComponent<tk2dSpriteAnimator>();
	}

    // Update is called once per frame
	void Update () {
	
	}

    public void Inicializar(int id)
    {
        Id = id;
        gameObject.name += "_" + id.ToString().PadLeft(2, '0');
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

    public void SubirNivel()
    {
        throw new System.NotImplementedException();
    }
}
