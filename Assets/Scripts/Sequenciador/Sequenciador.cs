using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using SSaME.Core;
using UnityEngine;

public class Sequenciador : MonoBehaviour
{
    public Camera Camera;
    public TipoEvolucaoPartida EvolucaoPartida = TipoEvolucaoPartida.Linear;
    public int DuracaoAtaque = 1;
    public int TempoEsperaAntesDeRecomecarReproducao = 1;

    // m�quina
    private GeradorAtaques geradorAtaques;
    private readonly List<Ataque> ataquesGerados = new List<Ataque>();
    private RepositorioPersonagens repositorioPersonagens;
    private IProgressaoPartida progressaoPartida;

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

    private IProgressaoPartida DefinirProgressaoPartida()
    {
        switch (EvolucaoPartida)
        {
            case TipoEvolucaoPartida.Linear: return new ProgressaoLinear(repositorioPersonagens, 3);
            case TipoEvolucaoPartida.Assimetrica: return new ProgressaoAssimetrica();
        }
        throw new InvalidOperationException("N�o existe progressor de partida do tipo " + EvolucaoPartida);
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
        progressaoPartida = DefinirProgressaoPartida();
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

//public static class ProgressaoPartidaFactory
//{
//    public static IProgressaoPartida CriarProgressor(TipoEvolucaoPartida evolucaoPartida)
//    {
//        switch (evolucaoPartida)
//        {
//                case TipoEvolucaoPartida.Linear:return new ProgressaoLinear();
//        }
//    }
//}