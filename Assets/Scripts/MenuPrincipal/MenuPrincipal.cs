using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AnimadorSelecao))]
public class MenuPrincipal : MonoBehaviour
{
    public GameObject BotaoNovo;
    public GameObject BotaoJogo;
    public GameObject BotaoSair;
    public string NomeLevelNovoJogo = "load_sceen";
    public float IntervaloEntreBotoesDica = 1f;
    public float IntervaloEntreDicas = 15f;

    private const int IdBotaoNovo = 1;
    private const int IdBotaoJogo = 2;
    private const int IdBotaoSair = 3;

    private AnimadorSelecao animadorSelecao;
    private bool jogadorPodeInteragir = true;
    private bool jogadorSelecionouUmaOpcao;
    private bool fecharJogo;


    private int ultimoBotao;
    private int penultimoBotao;
    private int clickCount;

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

    private IEnumerator DarDicaMenu()
    {
        jogadorPodeInteragir = false;
        animadorSelecao.AnimarSelecao(BotaoNovo);
        yield return new WaitForSeconds(IntervaloEntreBotoesDica);
        animadorSelecao.AnimarSelecao(BotaoJogo);
        jogadorPodeInteragir = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void BotaoNovoClick()
    {
        if (!jogadorPodeInteragir)
            return;

        Clicar(IdBotaoNovo);
        animadorSelecao.AnimarSelecao(BotaoNovo);
    }

    private void Clicar(int idBotao)
    {
        clickCount++;

        penultimoBotao = ultimoBotao;
        ultimoBotao = idBotao;
    }

    void BotaoJogoClick()
    {
        if (!jogadorPodeInteragir)
            return;

        Clicar(IdBotaoJogo);
        animadorSelecao.AnimarSelecao(BotaoJogo);

        VerificarCarregamentoDoMapa();
        VerificarSaida();
    }

    void BotaoSairClick()
    {
        if (!jogadorPodeInteragir)
            return;

        Clicar(IdBotaoSair);
        animadorSelecao.AnimarSelecao(BotaoSair);
    }

    private void VerificarCarregamentoDoMapa()
    {
        if (!JogadorCompletouUmaJogada())
            return;

        if (penultimoBotao == IdBotaoNovo && ultimoBotao == IdBotaoJogo)
        {
            jogadorSelecionouUmaOpcao = true;
            StartCoroutine(CarregarCenario());
        }
    }

    private bool JogadorCompletouUmaJogada()
    {
        return clickCount >= 2 && clickCount % 2 == 0;
    }

    private IEnumerator CarregarCenario()
    {
        yield return new WaitForSeconds(animadorSelecao.DuracaoAnimacao + .5f);
        Application.LoadLevel(NomeLevelNovoJogo);
    }

    private void VerificarSaida()
    {
        if (!JogadorCompletouUmaJogada())
            return;

        if (penultimoBotao == IdBotaoSair && ultimoBotao == IdBotaoJogo)
        {
            jogadorPodeInteragir = false;
            StartCoroutine(FecharEmAlgunsSegundos(1f));
        }
    }

    private IEnumerator FecharEmAlgunsSegundos(float tempo)
    {
        yield return new WaitForSeconds(tempo);
        Application.Quit();
    }
}
