using AllLogic.Sequenciador;
using UnityEngine;
using Messaging;

[RequireComponent(typeof(AnimadorSelecao))]
public class Personagem : MonoBehaviour, IPersonagemJogo
{
    private const int NivelInicial = 1;
    private const int VidaInicial = 100;

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
    private int vida = VidaInicial;

    public int Vida
    {
        get { return vida; }
        set { vida = value; }
    }
    

    [SerializeField]
    private Equipe equipe = Equipe.A;

    public AudioClip SomSelecao;
    public AudioClip SomAtaque;

    private tk2dSpriteAnimator spriteAnimator;
    private AnimadorSelecao animadorSelecao;
    private IndicadorNivelPersonagem indicadorNivel;
    private SeletorAnimacaoAtaque seletorAnimacaoAtaque;

    void Awake()
    {
    }
    
    // Use this for initialization
	void Start ()
	{
	    spriteAnimator = GetComponent<tk2dSpriteAnimator>();
        seletorAnimacaoAtaque = new SeletorAnimacaoAtaque(spriteAnimator);
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
        Messenger.Send(MessageType.JogadorSelecionado, new Message<Equipe>(equipe));
    }

    public void Atacar()
    {
        if (SomAtaque)
        {
            AudioSource.PlayClipAtPoint(SomAtaque, transform.position);
        }

        var clip = seletorAnimacaoAtaque.BuscarClipe(Nivel);
        spriteAnimator.Play(clip);
    }

    public void SubirNivel()
    {
        DefinirNivel(nivel + 1);
    }

    public void ResetarNivel()
    {
        DefinirNivel(NivelInicial);
    }

    public void AdicionarVida(int qtdVida)
    {
        if (qtdVida > 0)
        {
            Vida = Mathf.Clamp(Vida + qtdVida, 0, 100);
            Debug.Log(string.Format("Personagem {0}, vida: {1}", gameObject.name, Vida));
        }

        if (qtdVida == 0)
            Morrer();
    }

    private void Morrer()
    {
        Debug.Log(string.Format("Eu ({0}) morri.", gameObject.name));
    }

    private void DefinirNivel(int novoNivel)
    {
        nivel = novoNivel;
        indicadorNivel.AtualizarNivelPersonagem(novoNivel);
    }

    public Collider GetCollider()
    {
        return collider;
    }
}