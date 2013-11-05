using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequenciador : MonoBehaviour
{
    public static Sequenciador Instancia;

    public Camera Camera;
    public Personagem[] TimeA;
    public Personagem[] TimeB;
    public int DuracaoAtaque = 1;
    public int TempoEsperaAntesDeRecomecarReproducao = 1;

    // máquina
    private Arena arena;
    private GeradorAtaques geradorAtaques;
    private readonly List<Ataque> ataquesGerados = new List<Ataque>();
    private List<Personagem> todosOsPersonagens; 

    // jogador
    private Stack<Personagem> personagensSelecionados;
    private ValidadorAtaques validadorAtaques;
    private Sequencia sequenciaAtaques;

    void Awake()
    {
        Instancia = this;
    }

    // Use this for initialization
    void Start()
    {
        arena = new Arena();
        geradorAtaques = new GeradorAtaques(arena, new UnityRandomizer());
        validadorAtaques = new ValidadorAtaques(arena);
        sequenciaAtaques = new Sequencia();
        personagensSelecionados = new Stack<Personagem>(2);
        todosOsPersonagens = TimeA.Union(TimeB).ToList();

        // inicialização personagens do Time A
        foreach (var personagem in TimeA)
        {
            int idPersonagem = arena.AdicionarParticipanteAoTimeA();
            personagem.Inicializar(idPersonagem, Times.TimeA);
        }

        // inicialização personagens do Time B
        foreach (var personagem in TimeB)
        {
            int idPersonagem = arena.AdicionarParticipanteAoTimeB();
            personagem.Inicializar(idPersonagem, Times.TimeB);
        }

        StartCoroutine(ComecarProximaRodada());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                foreach (var personagem in TimeA)
                {
                    if (PersonagemFoiSelecionado(hit, personagem))
                    {
                        personagensSelecionados.Push(personagem);
                        personagem.Selecionar();
                        Debug.Log(string.Format("{0} selecionado", personagem.gameObject.name));
                        continue;
                    }
                }

                foreach (var personagem in TimeB)
                {
                    if (PersonagemFoiSelecionado(hit, personagem))
                    {
                        personagensSelecionados.Push(personagem);
                        personagem.Selecionar();
                        Debug.Log(string.Format("{0} selecionado", personagem.gameObject.name));
                    }
                }
            }
        }

        if (personagensSelecionados.Count == 2)
        {
            ValidarAtaque();
        }
    }

    private void ValidarAtaque()
    {
        var alvo = personagensSelecionados.Pop();
        var atacante = personagensSelecionados.Pop();
        var ataque = new Ataque(atacante.Id, alvo.Id, atacante.Time);

        if (validadorAtaques.AtaqueValido(ataque))
        {
            sequenciaAtaques.ArmazenarAtaque(ataque);
            if (sequenciaAtaques.Validar(ataquesGerados))
            {
                if (sequenciaAtaques.EstaCompleta(ataquesGerados))
                {
                    StartCoroutine(ComecarProximaRodada());
                }
            }
            else
            {
                StartCoroutine(ManipularErroAtaque());
            }
        }
        else
        {
            StartCoroutine(ManipularErroAtaque());
        }
    }

    private IEnumerator ManipularErroAtaque()
    {
        Debug.Log("VOCÊ ERROU!!!");

        ataquesGerados.Clear();
        sequenciaAtaques = new Sequencia();
        yield return new WaitForSeconds(TempoEsperaAntesDeRecomecarReproducao);

        StartCoroutine(ComecarProximaRodada());
    }

    private IEnumerator ComecarProximaRodada()
    {
        sequenciaAtaques = new Sequencia();
        yield return new WaitForSeconds(TempoEsperaAntesDeRecomecarReproducao);

        GerarAtaque();
        StartCoroutine(ReproduzirSequenciaAtaques());
    }

    private void GerarAtaque()
    {
        var ataque = geradorAtaques.GerarAtaque();
        ataquesGerados.Add(ataque);
        Debug.Log(ataque);
    }

    private bool PersonagemFoiSelecionado(RaycastHit hit, Personagem personagem)
    {
        return personagem.collider.transform == hit.transform;
    }

    private IEnumerator ReproduzirSequenciaAtaques()
    {
        Debug.Log("*Sequência de ataques:");
        foreach (var ataque in ataquesGerados.ToList())
        {
            StartCoroutine(ReproduzirAtaque(ataque));
            yield return new WaitForSeconds(DuracaoAtaque);
        }
    }

    private IEnumerator ReproduzirAtaque(Ataque ataque)
    {
        Debug.Log(ataque);

        var atacante = todosOsPersonagens.Find(p => p.Id == ataque.Atacante);
        var alvo = todosOsPersonagens.Find(p => p.Id == ataque.Alvo);

        atacante.Selecionar();
        yield return new WaitForSeconds(.5f);
        alvo.Selecionar();
    }
}