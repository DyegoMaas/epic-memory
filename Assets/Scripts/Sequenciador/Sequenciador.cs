using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequenciador : MonoBehaviour
{
    public Camera Camera;
    public Personagem[] TimeA;
    public Personagem[] TimeB;
    public int DuracaoAtaque = 1;
    public int TempoEsperaAntesDeRecomecarReproducao = 1;

    // máquina
    private Arena arena;
    private GeradorAtaques geradorAtaques;
    private readonly List<Ataque> ataquesGerados = new List<Ataque>();
    private Dictionary<int, Personagem> personagens;

    // jogador
    private Stack<Personagem> personagensSelecionados;
    private ValidadorAtaques validadorAtaques;
    private Sequencia sequenciaAtaques;
    private readonly List<int> touchesProcessed = new List<int>();
    private bool jogadorPodeInteragir;

    // Use this for initialization
    void Start()
    {
        arena = new Arena();
        geradorAtaques = new GeradorAtaques(arena, new UnityRandomizer());
        validadorAtaques = new ValidadorAtaques(arena);
        sequenciaAtaques = new Sequencia();
        personagensSelecionados = new Stack<Personagem>(2);
        personagens = new Dictionary<int, Personagem>(TimeA.Length + TimeB.Length);

        // inicialização personagens do Time A
        foreach (var personagem in TimeA)
        {
            int idPersonagem = arena.AdicionarParticipanteAoTimeA();
            personagem.Inicializar(idPersonagem, Times.TimeA);

            personagens.Add(idPersonagem, personagem);
        }

        // inicialização personagens do Time B
        foreach (var personagem in TimeB)
        {
            int idPersonagem = arena.AdicionarParticipanteAoTimeB();
            personagem.Inicializar(idPersonagem, Times.TimeB);

            personagens.Add(idPersonagem, personagem);
        }

        StartCoroutine(ComecarProximaRodada());
    }

    // Update is called once per frame
    void Update()
    {
        if (!jogadorPodeInteragir)
        {
            return;
        }

        Vector3 clickPosition;
        if (Click(out clickPosition))
        {
            Ray ray = Camera.ScreenPointToRay(clickPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                foreach (var personagem in TimeA)
                {
                    if (PersonagemFoiSelecionado(hit, personagem))
                    {
                        SelecionarPersonagem(personagem);
                        continue;
                    }
                }

                foreach (var personagem in TimeB)
                {
                    if (PersonagemFoiSelecionado(hit, personagem))
                    {
                        SelecionarPersonagem(personagem);
                        continue;
                    }
                }
            }
        }

        if (JogadorCompletouUmAtaque())
        {
            ValidarAtaque();
        }
    }
    
    private bool Click(out Vector3 clickPosition)
    {
        clickPosition = Vector3.zero;

        if (Input.GetButtonDown("Fire1"))
        {
            clickPosition = Input.mousePosition;
            return true;
        }

        if(Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            if (!touchesProcessed.Contains(touch.fingerId))
            {
                clickPosition = touch.position;
                touchesProcessed.Add(touch.fingerId);
            }
        }

        return false;
    }

    private bool PersonagemFoiSelecionado(RaycastHit hit, Personagem personagem)
    {
        return personagem.collider.transform == hit.transform;
    }

    private void SelecionarPersonagem(Personagem personagem)
    {
        personagensSelecionados.Push(personagem);
        personagem.Selecionar();
    }

    private bool JogadorCompletouUmAtaque()
    {
        return personagensSelecionados.Count == 2;
    }

    private void ValidarAtaque()
    {
        var alvo = personagensSelecionados.Pop();
        var atacante = personagensSelecionados.Pop();
        var ataque = new Ataque(atacante.Id, alvo.Id, atacante.Time);

        atacante.Atacar();

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
        jogadorPodeInteragir = false;
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

    private IEnumerator ReproduzirSequenciaAtaques()
    {
        jogadorPodeInteragir = false;
        foreach (var ataque in ataquesGerados.ToList())
        {
            StartCoroutine(ReproduzirAtaque(ataque));
            yield return new WaitForSeconds(DuracaoAtaque);
        }
        jogadorPodeInteragir = true;
    }

    private IEnumerator ReproduzirAtaque(Ataque ataque)
    {
        var atacante = personagens[ataque.Atacante];
        var alvo = personagens[ataque.Alvo];

        atacante.Selecionar();
        yield return new WaitForSeconds(.5f);
        alvo.Selecionar();
        atacante.Atacar();
    }
}