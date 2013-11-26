using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Progressao;
using Messaging;
using SSaME.Core.Sequenciador;
using UnityEngine;

[RequireComponent(typeof(ProgressaoPartidaFactory))]
public class Sequenciador : MonoBehaviour
{
    [SerializeField]
    private float duracaoAtaque = 1.5f;

    public float DuracaoAtaque
    {
        get { return duracaoAtaque * gerenciadorDificuldade.CoeficietenteFacilidade; }
        set { duracaoAtaque = value; }
    }

    [SerializeField]
    private float tempoEsperaAntesDeRecomecarReproducao = 1.5f;

    public float TempoEsperaAntesDeRecomecarReproducao
    {
        get { return tempoEsperaAntesDeRecomecarReproducao * gerenciadorDificuldade.CoeficietenteFacilidade; }
        set { tempoEsperaAntesDeRecomecarReproducao = value; }
    }

    public int NumeroTentativas = 3;
    private int numeroTentativasFaltando;

    // máquina
    private GeradorAtaques geradorAtaques;
    private readonly List<Ataque> ataquesGeradosPelaMaquina = new List<Ataque>();
    private readonly RepositorioPersonagens repositorioPersonagens = new RepositorioPersonagens();
    private IProgressaoPartida progressaoPartida;
    private IProgressaoPartidaFactory progressaoPartidaFactory;
    private GerenciadorDificuldade gerenciadorDificuldade;

    // jogador
    private readonly Stack<IPersonagem> personagensSelecionados = new Stack<IPersonagem>(2);
    private readonly ValidadorAtaques validadorAtaques = new ValidadorAtaques();
    private SequenciaAtaque sequenciaAtaqueAtaquesDoJogador = new SequenciaAtaque();
    private InputManager inputManager;
    private bool jogadorPodeInteragir;

    private bool jogoIniciado;

    // Use this for initialization
    void Start()
    {
        numeroTentativasFaltando = NumeroTentativas;

        inputManager = new InputManager(repositorioPersonagens);
        geradorAtaques = new GeradorAtaques(repositorioPersonagens, new UnityRandomizer());
        progressaoPartidaFactory = GetComponent<ProgressaoPartidaFactory>();
        gerenciadorDificuldade = FindObjectOfType(typeof (GerenciadorDificuldade)) as GerenciadorDificuldade;
        AdicionarTodosOsPersonagensNoRepositorio();
        Messenger.Subscribe(MessageType.NovoJogoIniciar, gameObject, "IniciarNovoJogo");

        StartCoroutine(AguardarNovoJogo());
    }

    private void AdicionarTodosOsPersonagensNoRepositorio()
    {
        var personagens = FindObjectsOfType(typeof(Personagem)) as Personagem[];
        if (personagens != null)
        {
            foreach (var personagem in personagens)
            {
                repositorioPersonagens.Adicionar(personagem);
            }
        }
    }
    
    private void IniciarNovoJogo()
    {
        jogoIniciado = true;
    }

    IEnumerator AguardarNovoJogo()
    {
        Messenger.Send(MessageType.NovoJogoAguardar);
        while (!jogoIniciado)
        {
            yield return null;
        }
        jogoIniciado = false;
        Debug.Log("começando nova partida");
        yield return StartCoroutine(ComecarProximaRodada());
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
            sequenciaAtaqueAtaquesDoJogador.ArmazenarAtaque(ataque);
            if (sequenciaAtaqueAtaquesDoJogador.Validar(ataquesGeradosPelaMaquina))
            {
                progressaoPartida.AtualizarProgressao(ataque);
                Messenger.Send(MessageType.AtaqueDesferido, new Message<Ataque>(ataque));

                if (sequenciaAtaqueAtaquesDoJogador.EstaCompleta(ataquesGeradosPelaMaquina))
                {
                    Messenger.Send(MessageType.JogadaCompleta);
                    StartCoroutine(ComecarProximaRodada());
                }
            }
            else
            {
                InvalidarAtaque(ataque);
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
        sequenciaAtaqueAtaquesDoJogador = new SequenciaAtaque();

        progressaoPartida = progressaoPartidaFactory.CriarProgressorPartida(repositorioPersonagens);
        yield return new WaitForSeconds(TempoEsperaAntesDeRecomecarReproducao);
        progressaoPartida.ResetarProgressoPartida();

        GerarAtaque();
        StartCoroutine(ReproduzirSequenciaAtaques());
    }

    private IEnumerator ManipularErroAtaque()
    {
        PrepararNovaTentativa();

        numeroTentativasFaltando--;
        if (FimdeJogo())
        {
            Messenger.Send(MessageType.GameOver);
            Messenger.Send(MessageType.PerfilJogadorAtivado, new Message<PerfilJogadorAtivo>(PerfilJogadorAtivo.Maquina));
            PrepararNovoJogo();

            yield return new WaitForSeconds(TempoEsperaAntesDeRecomecarReproducao);
            StartCoroutine(AguardarNovoJogo());
        }
        else
        {
            Messenger.Send(MessageType.ErroJogador, new Message<int>(numeroTentativasFaltando));
            Messenger.Send(MessageType.PerfilJogadorAtivado, new Message<PerfilJogadorAtivo>(PerfilJogadorAtivo.Maquina));
            
            yield return new WaitForSeconds(TempoEsperaAntesDeRecomecarReproducao);
            StartCoroutine(ReproduzirSequenciaAtaques());
        }
    }

    private bool FimdeJogo()
    {
        return numeroTentativasFaltando == 0;
    }

    private void PrepararNovaTentativa()
    {
        progressaoPartida.ResetarProgressoPartida();
        jogadorPodeInteragir = false;
        sequenciaAtaqueAtaquesDoJogador = new SequenciaAtaque();
    }

    private void PrepararNovoJogo()
    {
        numeroTentativasFaltando = NumeroTentativas;
        ataquesGeradosPelaMaquina.Clear();
    }

    private void GerarAtaque()
    {
        var ataque = geradorAtaques.GerarAtaque();
        ataquesGeradosPelaMaquina.Add(ataque);
    }

    private void InvalidarAtaque(Ataque ataque)
    {
        sequenciaAtaqueAtaquesDoJogador.RemoverAtaque(ataque);
    }

    private IEnumerator ReproduzirSequenciaAtaques()
    {
        jogadorPodeInteragir = false;
        foreach (var ataque in ataquesGeradosPelaMaquina.ToList())
        {
            yield return new WaitForSeconds(DuracaoAtaque / 2f);
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