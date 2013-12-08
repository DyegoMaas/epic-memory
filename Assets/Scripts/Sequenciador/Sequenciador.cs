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
        get { return tempoEsperaAntesDeRecomecarReproducao * gerenciadorDificuldade.CoeficienteFacilidade; }
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
    
    private SequenciaAtaque sequenciaAtaqueAtaquesDoJogador;
    private readonly List<Ataque> ataquesGeradosPelaMaquina = new List<Ataque>();
    private IProgressaoPartida progressaoPartida;
    private IProgressaoPartidaFactory progressaoPartidaFactory;

    private bool jogadorPodeInteragir;
    private bool jogoIniciado;
    private SelecaoPersonagens selecaoPersonagens;

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
        Messenger.Subscribe(MessageType.NovoJogoIniciar, gameObject, "IniciarNovoJogo");

        StartCoroutine(AguardarNovoJogo());
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

        if (selecaoPersonagens.AtaquesGerados.Count > 0)
        {
            var ataqueGerado = selecaoPersonagens.AtaquesGerados.Dequeue();
            ValidarAtaque(ataqueGerado);
        }
    }
    
    private void ValidarAtaque(Ataque ataque)
    {
        ataque.Atacante.Atacar();

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
            contadorTentativas.ErroJogador();
            Messenger.Send(MessageType.ErroJogador, new Message<int>(contadorTentativas.NumeroTentativasRestantes));
            Messenger.Send(MessageType.PerfilJogadorAtivado, new Message<PerfilJogadorAtivo>(PerfilJogadorAtivo.Maquina));
            
            yield return new WaitForSeconds(TempoEsperaAntesDeRecomecarReproducao);
            StartCoroutine(ReproduzirSequenciaAtaques());
        }
    }

    private bool FimdeJogo()
    {
        return contadorTentativas.NumeroTentativasRestantes == 0;
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