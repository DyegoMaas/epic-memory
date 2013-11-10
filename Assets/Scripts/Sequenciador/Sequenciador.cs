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
    public float DuracaoAtaque = 1.5f;
    public float TempoEsperaAntesDeRecomecarReproducao = 1.5f;

    // máquina
    private GeradorAtaques geradorAtaques;
    private readonly List<Ataque> ataquesGerados = new List<Ataque>();
    private readonly RepositorioPersonagens repositorioPersonagens = new RepositorioPersonagens();
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
        inputManager = new InputManager(repositorioPersonagens);
        geradorAtaques = new GeradorAtaques(repositorioPersonagens, new UnityRandomizer());
        progressaoPartidaFactory = GetComponent<ProgressaoPartidaFactory>();
        AdicionarTodosOsPersonagensNoRepositorio();

        StartCoroutine(ComecarProximaRodada());
    }

    private void AdicionarTodosOsPersonagensNoRepositorio()
    {
        var personagens = FindObjectsOfType(typeof (Personagem)) as Personagem[];
        if (personagens != null)
        {
            foreach (var personagem in personagens)
            {
                repositorioPersonagens.Adicionar(personagem);
            }
        }
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

    private bool OJogadorEscolheuUmPersonagemDoMesmoTime(IPersonagem personagem)
    {
        var personagemJaSelecionado = personagensSelecionados.Pop();
        bool mesmoTime = personagem.Equipe == personagemJaSelecionado.Equipe;
        personagensSelecionados.Push(personagemJaSelecionado);

        return mesmoTime;
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
                Messenger.Send(MessageType.AtaqueDesferido, new Message<Ataque>(ataque));

                if (sequenciaAtaques.EstaCompleta(ataquesGerados))
                {
                    Messenger.Send(MessageType.JogadaCompleta);
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

    private IEnumerator ComecarProximaRodada()
    {
        Messenger.Send(MessageType.PerfilJogadorAtivado,
                            new Message<PerfilJogadorAtivo>(PerfilJogadorAtivo.Maquina));
        sequenciaAtaques = new Sequencia();

        progressaoPartida = progressaoPartidaFactory.CriarProgressorPartida(repositorioPersonagens);
        yield return new WaitForSeconds(TempoEsperaAntesDeRecomecarReproducao);
        progressaoPartida.ResetarProgressoPartida();

        GerarAtaque();
        StartCoroutine(ReproduzirSequenciaAtaques());
    }

    private IEnumerator ManipularErroAtaque()
    {
        jogadorPodeInteragir = false;
        ataquesGerados.Clear();
        sequenciaAtaques = new Sequencia();
        progressaoPartida.ResetarProgressoPartida();
        Messenger.Send(MessageType.GameOver);

        yield return new WaitForSeconds(TempoEsperaAntesDeRecomecarReproducao);

        StartCoroutine(ComecarProximaRodada());
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
            yield return new WaitForSeconds(DuracaoAtaque);
            yield return StartCoroutine(ReproduzirAtaque(ataque));
        }
        jogadorPodeInteragir = true;

        const float tempoMostrandoONivel = .5f;
        yield return new WaitForSeconds(tempoMostrandoONivel);
        progressaoPartida.ResetarProgressoPartida();

        Messenger.Send(MessageType.PerfilJogadorAtivado,
                            new Message<PerfilJogadorAtivo>(PerfilJogadorAtivo.Jogador));
    }

    private IEnumerator ReproduzirAtaque(Ataque ataque)
    {
        ataque.Atacante.Selecionar();
        yield return new WaitForSeconds(.5f);
        ataque.Alvo.Selecionar();

        ataque.Atacante.Atacar();
        progressaoPartida.AtualizarProgressao(ataque);
        Messenger.Send(MessageType.AtaqueDesferido);
    }
}