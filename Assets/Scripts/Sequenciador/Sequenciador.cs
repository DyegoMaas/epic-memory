using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
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
    
    private SequenciaAtaque sequenciaAtaqueAtaquesDoJogador;
    private readonly List<Ataque> ataquesGeradosPelaMaquina = new List<Ataque>();
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
        sequenciaAtaqueAtaquesDoJogador = sequenciaAtaqueFactory.CriarSequenciaAtaque();

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
        Messenger.Send(MessageType.PerfilJogadorAtivado, new Message<PerfilJogadorAtivo>(PerfilJogadorAtivo.Maquina));
        sequenciaAtaqueAtaquesDoJogador = sequenciaAtaqueFactory.CriarSequenciaAtaque();
        progressaoPartida = progressaoPartidaFactory.CriarProgressorPartida();
        yield return new WaitForSeconds(TempoEsperaAntesDeRecomecarReproducao);
        progressaoPartida.ResetarProgressoPartida();

        GerarAtaque();
        StartCoroutine(ReproduzirSequenciaAtaques());
    }

    private IEnumerator ManipularErroAtaque()
    {
        PrepararNovaTentativa();

        if (gerenciadorEstadoJogo.FimDeJogo())
        {
            Messenger.Send(MessageType.GameOver);
            Messenger.Send(MessageType.PerfilJogadorAtivado, new Message<PerfilJogadorAtivo>(PerfilJogadorAtivo.Maquina));
            PrepararNovoJogo();

            yield return new WaitForSeconds(TempoEsperaAntesDeRecomecarReproducao);
            StartCoroutine(AguardarNovoJogo());
        }
        else
        {
            contadorTentativas.ErroJogador();
            Messenger.Send(MessageType.ErroJogador, new Message<int>(contadorTentativas.NumeroTentativasRestantes));
            Messenger.Send(MessageType.PerfilJogadorAtivado, new Message<PerfilJogadorAtivo>(PerfilJogadorAtivo.Maquina));
            
            yield return new WaitForSeconds(TempoEsperaAntesDeRecomecarReproducao);
            StartCoroutine(ReproduzirSequenciaAtaques());
        }
    }

    private void PrepararNovaTentativa()
    {
        progressaoPartida.ResetarProgressoPartida();
        jogadorPodeInteragir = false;
        sequenciaAtaqueAtaquesDoJogador = new SequenciaAtaque();
    }

    private void PrepararNovoJogo()
    {
        contadorTentativas.Resetar();
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
            yield return new WaitForSeconds(duracaoAtaque / 2f);
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