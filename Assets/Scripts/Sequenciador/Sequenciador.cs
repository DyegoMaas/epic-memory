using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Progressao;
using Messaging;
using SSaME.Core;
using UnityEngine;

[RequireComponent(typeof(ProgressaoPartidaFactory))]
public class Sequenciador : MonoBehaviour
{
    public Camera Camera;
    public int DuracaoAtaque = 1;
    public int TempoEsperaAntesDeRecomecarReproducao = 1;

    // máquina
    private GeradorAtaques geradorAtaques;
    private readonly List<Ataque> ataquesGerados = new List<Ataque>();
    private RepositorioPersonagens repositorioPersonagens;
    private IProgressaoPartida progressaoPartida;
    private IProgressaoPartidaFactory progressaoPartidaFactory;

    // jogador
    private readonly Stack<IPersonagem> personagensSelecionados = new Stack<IPersonagem>(2);
    private readonly ValidadorAtaques validadorAtaques = new ValidadorAtaques();
    private Sequencia sequenciaAtaques = new Sequencia();
    private InputManager inputManager;
    private bool jogadorPodeInteragir;

    // Use this for initialization
    void Start()
    {
        repositorioPersonagens = new RepositorioPersonagens();
        inputManager = new InputManager(repositorioPersonagens);
        geradorAtaques = new GeradorAtaques(repositorioPersonagens, new UnityRandomizer());
        progressaoPartidaFactory = GetComponent<ProgressaoPartidaFactory>();

        var personagens = FindObjectsOfType(typeof(Personagem)) as Personagem[];
        if (personagens != null)
        {
            foreach (var personagem in personagens)
            {
                repositorioPersonagens.Adicionar(personagem);
            }
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

        IPersonagem personagem;
        if (inputManager.Click(out personagem))
        {
            SelecionarPersonagem(personagem);
        }

        if (JogadorCompletouUmAtaque())
        {
            ValidarAtaque();
        }
    }

    private bool OJogadorEscolheuUmPersonagemDoMesmoTime(IPersonagem personagem)
    {
        var personagemJaSelecionado = personagensSelecionados.Pop();
        bool mesmoTime = personagem.Time == personagemJaSelecionado.Time;
        personagensSelecionados.Push(personagemJaSelecionado);

        return mesmoTime;
    }

    private void SelecionarPersonagem(IPersonagem personagem)
    {
        if (personagensSelecionados.Count == 1)
        {
            if (OJogadorEscolheuUmPersonagemDoMesmoTime(personagem))
            {
                return;
            }
        }

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
                progressaoPartida.AtualizarProgressao(ataque);

                if (sequenciaAtaques.EstaCompleta(ataquesGerados))
                {
                    Messenger.Broadcast(MessageType.AtaqueDesferido);
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
        Messenger.Broadcast(MessageType.GameOver);

        yield return new WaitForSeconds(TempoEsperaAntesDeRecomecarReproducao);

        StartCoroutine(ComecarProximaRodada());
    }

    private IEnumerator ComecarProximaRodada()
    {
        progressaoPartida = progressaoPartidaFactory.CriarProgressorPartida(repositorioPersonagens);
        ResetarNivelPersonagens();
        sequenciaAtaques = new Sequencia();
        yield return new WaitForSeconds(TempoEsperaAntesDeRecomecarReproducao);

        GerarAtaque();
        StartCoroutine(ReproduzirSequenciaAtaques());
    }

    private void ResetarNivelPersonagens()
    {
        repositorioPersonagens.BuscarTodos().ForEach(p => p.ResetarNivel());
    }

    private void GerarAtaque()
    {
        var ataque = geradorAtaques.GerarAtaque();
        ataquesGerados.Add(ataque);
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