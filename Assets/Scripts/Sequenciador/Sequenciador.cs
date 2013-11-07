using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SSaME.Core;
using UnityEngine;

public class Sequenciador : MonoBehaviour
{
    public Camera Camera;
    public int DuracaoAtaque = 1;
    public int TempoEsperaAntesDeRecomecarReproducao = 1;

    // m�quina
    private GeradorAtaques geradorAtaques;
    private readonly List<Ataque> ataquesGerados = new List<Ataque>();
    private RepositorioPersonagens repositorioPersonagens;

    // jogador
    private Stack<IPersonagem> personagensSelecionados;
    private ValidadorAtaques validadorAtaques;
    private Sequencia sequenciaAtaques = new Sequencia();
    private readonly List<int> touchesProcessed = new List<int>();
    private bool jogadorPodeInteragir;

    // Use this for initialization
    void Start()
    {
        var arena = new Arena();
        geradorAtaques = new GeradorAtaques(arena, new UnityRandomizer());
        validadorAtaques = new ValidadorAtaques();
        
        personagensSelecionados = new Stack<IPersonagem>(2);
        repositorioPersonagens = new RepositorioPersonagens();

        var personagens = FindObjectsOfType(typeof(Personagem)) as Personagem[];
        if (personagens != null)
        {
            foreach (var personagem in personagens)
            {
                ConsolidarPersonagem(personagem, arena);
                repositorioPersonagens.Adicionar(personagem);
            }
        }

        StartCoroutine(ComecarProximaRodada());
    }

    private void ConsolidarPersonagem(Personagem personagem, Arena arena)
    {
        arena.AdicionarPersonagem(personagem);
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
                foreach (var personagem in repositorioPersonagens.BuscarTodos())
                {
                    if (PersonagemFoiSelecionado(hit, personagem))
                    {
                        if (personagensSelecionados.Count == 1)
                        {
                            if (OJogadorEscolheuUmPersonagemDoMesmoTime(personagem))
                            {
                                continue;
                            }
                        }

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

    private bool OJogadorEscolheuUmPersonagemDoMesmoTime(IPersonagem personagem)
    {
        var personagemJaSelecionado = personagensSelecionados.Pop();
        bool mesmoTime = personagem.Time == personagemJaSelecionado.Time;
        personagensSelecionados.Push(personagemJaSelecionado);

        return mesmoTime;
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
        var ataque = new Ataque(atacante, alvo);

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
        ataque.Atacante.Selecionar();
        yield return new WaitForSeconds(.5f);
        ataque.Alvo.Selecionar();
        ataque.Atacante.Atacar();
    }
}