using System.Collections;
using EpicMemory.Sequenciador;
using Messaging;
using UnityEngine;

[RequireComponent(typeof(SelecaoPersonagens))]
public class ReprodutorBatalha : InjectionBehaviour
{
    [SerializeField]
    private float duracaoAtaque = 1.5f;

    public float DuracaoAtaque
    {
        get { return duracaoAtaque * gerenciadorDificuldade.CoeficienteFacilidade; }
        set { duracaoAtaque = value; }
    }

    [InjectedDependency] private IProgressaoNivelPartida progressaoPartida;
    [InjectedDependency] private IProgressaoBatalha progressaoBatalha;
    [InjectedDependency] private GerenciadorPerfis gerenciadorPerfis;
    [InjectedDependency] private GerenciadorDificuldade gerenciadorDificuldade;

    private SelecaoPersonagens selecaoPersonagens;
    
    protected override void StartOverride()
    {
        selecaoPersonagens = GetComponent<SelecaoPersonagens>();
    }

    public IEnumerator ReproduzirSequenciaAtaques(SequenciaAtaque sequenciaMaquina)
    {
        selecaoPersonagens.Desabilitar();
        int indiceAtaque = 0;
        foreach (var ataque in sequenciaMaquina.ToList())
        {
            yield return new WaitForSeconds(DuracaoAtaque / 2f);
            yield return StartCoroutine(ReproduzirAtaque(ataque));
            progressaoBatalha.AtualizarPercentual(sequenciaMaquina, ++indiceAtaque);
        }
        selecaoPersonagens.Habilitar();

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