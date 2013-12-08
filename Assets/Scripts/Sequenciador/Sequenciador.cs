using System.Collections;
using Assets.Scripts;
using Assets.Scripts.Progressao;
using EpicMemory.Sequenciador;
using Messaging;
using UnityEngine;

[RequireComponent(typeof(ConfiguradorTentativas))]
[RequireComponent(typeof(ProgressaoPartidaFactory))]
[RequireComponent(typeof(SelecaoPersonagens))]
[RequireComponent(typeof(InicializadorRepositorio))]
[RequireComponent(typeof(EstadoJogoScript))]
public class Sequenciador : InjectionBehaviour
{
    [SerializeField]
    private float duracaoAtaque = 1.5f;

    public float DuracaoAtaque
    {
        get { return duracaoAtaque * gerenciadorDificuldade.CoeficienteFacilidade; }
        set { duracaoAtaque = value; }
    }

    [SerializeField]
    private float tempoEsperaAntesDeRecomecarReproducao = 1.5f;

    public float TempoEsperaAntesDeRecomecarReproducao
    {
        //get { return tempoEsperaAntesDeRecomecarReproducao * gerenciadorDificuldade.CoeficienteFacilidade; }
        get { return tempoEsperaAntesDeRecomecarReproducao; }
        set { tempoEsperaAntesDeRecomecarReproducao = value; }
    }

    public float TempoEsperaComecarJogo = 1f;

    [InjectedDependency] private IGeradorAtaques geradorAtaques;
    [InjectedDependency] private RepositorioPersonagens repositorioPersonagens;
    [InjectedDependency] private GerenciadorDificuldade gerenciadorDificuldade;
    [InjectedDependency] private ValidadorAtaques validadorAtaques;
    [InjectedDependency] private SequenciaAtaqueFactory sequenciaAtaqueFactory;
    [InjectedDependency] private IInputManager inputManager;
    [InjectedDependency] private IContadorTentativas contadorTentativas;
    [InjectedDependency] private GerenciadorEstadoJogo gerenciadorEstadoJogo;
    [InjectedDependency] private GerenciadorGUI gerenciadorGui;
    [InjectedDependency] private GerenciadorPontuacao gerenciadorPontuacao;
    [InjectedDependency] private GerenciadorPerfis gerenciadorPerfis;
    
    private SequenciaAtaque sequenciaAtaquesDoJogador;
    private SequenciaAtaque sequenciaAtaquesDaMaquina;
    private IProgressaoPartida progressaoPartida;
    private IProgressaoPartidaFactory progressaoPartidaFactory;
    private SelecaoPersonagens selecaoPersonagens;

    private bool jogadorPodeInteragir;

    protected override void StartOverride()
    {
        StartCoroutine(MyStart());
    }

    IEnumerator MyStart()
    {
        selecaoPersonagens = GetComponent<SelecaoPersonagens>();
        progressaoPartidaFactory = GetComponent<ProgressaoPartidaFactory>();
        sequenciaAtaquesDoJogador = sequenciaAtaqueFactory.CriarSequenciaAtaque();
        sequenciaAtaquesDaMaquina = sequenciaAtaqueFactory.CriarSequenciaAtaque();

        yield return new WaitForSeconds(TempoEsperaComecarJogo);

        StartCoroutine(AguardarNovoJogo());
    }

    IEnumerator AguardarNovoJogo()
    {
        gerenciadorGui.MostrarBotaoComecar();
        while (EstadoAtualJogo() != EstadoJogo.Iniciado)
        {
            yield return null;
        }
        gerenciadorEstadoJogo.AguardarNovoJogo();

        Debug.Log("começando nova partida");
        yield return StartCoroutine(ComecarProximaRodada());
    }

    private EstadoJogo EstadoAtualJogo()
    {
        return gerenciadorEstadoJogo.EstadoJogo;
    }

    // Update is called once per frame
    void Update()
    {
        if (!jogadorPodeInteragir)
        {
            return;
        }

        if (selecaoPersonagens.AtaquesGerados.Count > 0)
        {
            var ataqueGerado = selecaoPersonagens.AtaquesGerados.Dequeue();
            ataqueGerado.Atacante.Atacar();

            ValidarAtaque(ataqueGerado);
        }
    }
    
    private void ValidarAtaque(Ataque ataque)
    {
        if (validadorAtaques.AtaqueValido(ataque))
        {
            sequenciaAtaquesDoJogador.ArmazenarAtaque(ataque);
            if (sequenciaAtaquesDoJogador.Validar(sequenciaAtaquesDaMaquina))
            {
                progressaoPartida.AtualizarProgressao(ataque);
                Messenger.Send(MessageType.AtaqueDesferido, new Message<Ataque>(ataque));

                if (sequenciaAtaquesDoJogador.EstaCompleta(sequenciaAtaquesDaMaquina))
                {
                    Pontuar();
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

    private void Pontuar()
    {
        Messenger.Send(MessageType.JogadaCompleta);
        gerenciadorPontuacao.Pontuar();
    }

    private IEnumerator ComecarProximaRodada()
    {
        gerenciadorPerfis.AtivarPerfilMaquina();
        sequenciaAtaquesDoJogador = sequenciaAtaqueFactory.CriarSequenciaAtaque();
        progressaoPartida = progressaoPartidaFactory.CriarProgressorPartida();
        yield return new WaitForSeconds(TempoEsperaAntesDeRecomecarReproducao);
        progressaoPartida.ResetarProgressoPartida();

        GerarAtaque();
        StartCoroutine(ReproduzirSequenciaAtaques());
    }

    private IEnumerator ManipularErroAtaque()
    {
        contadorTentativas.ErroJogador();
        PrepararNovaTentativa();

        if (gerenciadorEstadoJogo.FimDeJogo())
        {
            PrepararNovoJogo();

            yield return new WaitForSeconds(TempoEsperaAntesDeRecomecarReproducao);
            StartCoroutine(AguardarNovoJogo());
        }
        else
        {
            Messenger.Send(MessageType.ErroJogador, new Message<int>(contadorTentativas.NumeroTentativasRestantes));
            
            yield return new WaitForSeconds(TempoEsperaAntesDeRecomecarReproducao);
            StartCoroutine(ReproduzirSequenciaAtaques());
        }
    }

    private void PrepararNovaTentativa()
    {
        progressaoPartida.ResetarProgressoPartida();
        jogadorPodeInteragir = false;
        sequenciaAtaquesDoJogador = sequenciaAtaqueFactory.CriarSequenciaAtaque();
        gerenciadorPerfis.AtivarPerfilMaquina();
    }

    private void PrepararNovoJogo()
    {
        Messenger.Send(MessageType.GameOver);

        gerenciadorEstadoJogo.PrepararNovoJogo();
        sequenciaAtaquesDaMaquina = sequenciaAtaqueFactory.CriarSequenciaAtaque();
    }

    private void GerarAtaque()
    {
        var ataque = geradorAtaques.GerarAtaque();
        sequenciaAtaquesDaMaquina.ArmazenarAtaque(ataque);
    }

    private void InvalidarAtaque(Ataque ataque)
    {
        sequenciaAtaquesDoJogador.RemoverAtaque(ataque);
    }

    private IEnumerator ReproduzirSequenciaAtaques()
    {
        jogadorPodeInteragir = false;
        foreach (var ataque in sequenciaAtaquesDaMaquina.ToList())
        {
            yield return new WaitForSeconds(duracaoAtaque / 2f);
            yield return StartCoroutine(ReproduzirAtaque(ataque));
        }
        jogadorPodeInteragir = true;

        const float tempoMostrandoONivel = .5f;
        yield return new WaitForSeconds(tempoMostrandoONivel);
        progressaoPartida.ResetarProgressoPartida();

        gerenciadorPerfis.AtivarPerfilJogador();
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