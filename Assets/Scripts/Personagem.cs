using SSaME.Core;
using UnityEngine;
using System.Collections;
using Messaging;

[RequireComponent(typeof(AnimadorSelecao))]
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
    private AnimadorSelecao animadorSelecao;
    private IndicadorNivelPersonagem indicadorNivel;

    void Awake()
    {
    }
    
    // Use this for initialization
	void Start ()
	{
	    spriteAnimator = GetComponent<tk2dSpriteAnimator>();
	    animadorSelecao = GetComponent<AnimadorSelecao>();
	    indicadorNivel = GetComponentInChildren<IndicadorNivelPersonagem>();
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

        animadorSelecao.AnimarSelecao();
    }

    private void NotificarSelecaoPersonagem()
    {
        Messenger.Broadcast(MessageType.JogadorSelecionado, new Message<Equipe>(equipe));
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
        DefinirNivel(nivel + 1);
    }

    public void ResetarNivel()
    {
        DefinirNivel(NivelInicial);
    }

    private void DefinirNivel(int novoNivel)
    {
        Debug.Log("novo nivel");
        nivel = novoNivel;
        indicadorNivel.AtualizarNivelPersonagem(novoNivel);
    }

    public Collider GetCollider()
    {
        return collider;
    }
}
