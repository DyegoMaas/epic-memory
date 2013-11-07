using SSaME.Core;
using UnityEngine;
using System.Collections;
using Time = SSaME.Core.Time;

public class Personagem : MonoBehaviour, IPersonagem {

    public Time Time { get; private set; }
    public int Id { get; private set; }

    [SerializeField]
    private int nivel;

    public int Nivel
    {
        get { return nivel; }
        set { nivel = value; }
    }

    public Time TimePersonagem = Time.A;
    public AudioClip SomSelecao;
    public AudioClip SomAtaque;

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
        if (SomSelecao)
        {
            AudioSource.PlayClipAtPoint(SomSelecao, transform.position);
        }
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
        if (SomAtaque)
        {
            AudioSource.PlayClipAtPoint(SomAtaque, transform.position);
        }
        spriteAnimator.Play("Ataque");
    }

    public void SubirNivel()
    {
        nivel++;
        Debug.Log(string.Format("{0} subiu para o nível {1}", gameObject.name, nivel));
    }

    public void ResetarNivel()
    {
        nivel = 0;
    }

    public Collider GetCollider()
    {
        return collider;
    }
}
