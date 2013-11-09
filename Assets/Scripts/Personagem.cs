using SSaME.Core;
using UnityEngine;
using System.Collections;
using Messaging;

public class Personagem : MonoBehaviour, IPersonagem
{
    private const int NivelInicial = 1;

    public int Id { get; private set; }

    public Equipe Equipe
    {
        get { return equipe; }
        set { equipe = value; }
    }

    [SerializeField]
    private int nivel = NivelInicial;

    public int Nivel
    {
        get { return nivel; }
        set { nivel = value; }
    }

    [SerializeField]
    private Equipe equipe = Equipe.A;

    public AudioClip SomSelecao;
    public AudioClip SomAtaque;

    private tk2dSpriteAnimator spriteAnimator;

    void Awake()
    {
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

        NotificarSelecaoPersonagem();

        StartCoroutine(AnimarSelecao());
    }

    private void NotificarSelecaoPersonagem()
    {
        Messenger.Broadcast(MessageType.JogadorSelecionado, new Message<Equipe>(equipe));
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
        nivel = NivelInicial;
    }

    public Collider GetCollider()
    {
        return collider;
    }
}
