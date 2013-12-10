using System.Collections;
using Assets.Scripts;
using EpicMemory.Sequenciador;
using Messaging;
using UnityEngine;

[RequireComponent(typeof(ConfiguradorTentativas))]
[RequireComponent(typeof(SelecaoPersonagens))]
[RequireComponent(typeof(InicializadorRepositorio))]
[RequireComponent(typeof(EstadoJogoScript))]
[RequireComponent(typeof(ReprodutorBatalha))]
public class Sequenciador : InjectionBehaviour
{
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
    [InjectedDependency] private IProgressaoNivelPartida progressaoNivelPartida;
    
    private SequenciaAtaque sequenciaAtaquesDoJogador;
    private SequenciaAtaque sequenciaAtaquesDaMaquina;
    private SelecaoPersonagens selecaoPersonagens;
    private ReprodutorBatalha reprodutorBatalha;

    protected override void StartOverride()
    {
        StartCoroutine(MyStart());
    }

    IEnumerator MyStart()
    {
        selecaoPersonagens = GetComponent<SelecaoPersonagens>();
        reprodutorBatalha = GetComponent<ReprodutorBatalha>();
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

        Debug.Log("comešando nova partida");
        yield return StartCoroutine(ComecarProximaRodada());
    }

    private EstadoJogo EstadoAtualJogo()
    {
        return gerenciadorEstadoJogo.EstadoJogo;
    }

    // Update is called once per frame
    void Update()
    {
        if (selecaoPersonagens.AtaquesGerados.Count > 0)
        {
            var ataqueGerado = selecaoPersonagens.AtaquesGerados.Dequeue();
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
                progressaoNivelPartida.AtualizarProgressao(ataque);
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
        yield return new WaitForSeconds(TempoEsperaAntesDeRecomecarReproducao);
        progressaoNivelPartida.ResetarProgressoPartida();

        GerarAtaque();
        StartCoroutine(reprodutorBatalha.ReproduzirSequenciaAtaques(sequenciaAtaquesDaMaquina));
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
            StartCoroutine(reprodutorBatalha.ReproduzirSequenciaAtaques(sequenciaAtaquesDaMaquina));
        }
    }

    private void PrepararNovaTentativa()
    {
        progressaoNivelPartida.ResetarProgressoPartida();
        selecaoPersonagens.Desabilitar();
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
}