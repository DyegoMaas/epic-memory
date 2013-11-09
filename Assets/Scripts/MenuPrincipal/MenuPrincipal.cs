using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimadorSelecao))]
public class MenuPrincipal : MonoBehaviour
{
    public string NomeLevelNovoJogo = "load_sceen";
    public GameObject BotaoNovo;
    public GameObject BotaoJogo;
    public float IntervaloEntreBotoesDica = 1f;
    public float IntervaloEntreDicas = 15f;

    private const int IdBotaoNovo = 1;
    private const int IdBotaoJogo = 2;

    private readonly Stack<int> clicks = new Stack<int>();
    private AnimadorSelecao animadorSelecao;
    private bool jogadorPodeInteragir = true;
    private bool jogadorSelecionouUmaOpcao = false;

    // Use this for initialization
    void Start()
    {
        animadorSelecao = GetComponent<AnimadorSelecao>();
        StartCoroutine(DarDicaACadaXSegundos());
    }

    private IEnumerator DarDicaACadaXSegundos()
    {
        yield return new WaitForSeconds(5);
        while (!jogadorSelecionouUmaOpcao)
        {
            yield return StartCoroutine(DarDicaMenu());
            yield return new WaitForSeconds(IntervaloEntreDicas);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void BotaoNovoClick()
    {
        if (!jogadorPodeInteragir)
            return;

        clicks.Push(IdBotaoNovo);
        animadorSelecao.AnimarSelecao(BotaoNovo);
    }

    void BotaoJogoClick()
    {
        if (!jogadorPodeInteragir)
            return;

        clicks.Push(IdBotaoJogo);
        animadorSelecao.AnimarSelecao(BotaoJogo);

        VerificarCarregamentoDoMapa();
    }

    private void VerificarCarregamentoDoMapa()
    {
        if (clicks.Count % 2 != 0)
            return;

        var ultimoClick = clicks.Pop();
        var penultimoClick = clicks.Pop();

        if (penultimoClick == IdBotaoNovo && ultimoClick == IdBotaoJogo)
        {
            jogadorSelecionouUmaOpcao = true;
            StartCoroutine(CarregarCenario());
        }
    }

    private IEnumerator CarregarCenario()
    {
        yield return new WaitForSeconds(animadorSelecao.DuracaoAnimacao + .5f);
        Application.LoadLevel(NomeLevelNovoJogo);
    }


    private IEnumerator DarDicaMenu()
    {
        jogadorPodeInteragir = false;
        animadorSelecao.AnimarSelecao(BotaoNovo);
        yield return new WaitForSeconds(IntervaloEntreBotoesDica);
        animadorSelecao.AnimarSelecao(BotaoJogo);
        jogadorPodeInteragir = true;
    }
}
