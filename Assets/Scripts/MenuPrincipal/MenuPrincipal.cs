using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AnimadorSelecao))]
public class MenuPrincipal : MonoBehaviour
{
    public GameObject BotaoNovo;
    public GameObject BotaoJogo;
    public GameObject BotaoSair;
    public GameObject CarregandoTextMesh;
    public string NomeLevelNovoJogo = "load_sceen";
    public float IntervaloEntreBotoesDica = 1f;
    public float IntervaloEntreDicas = 15f;
    public float TempoParaNovoJogo = 1f;

    private const int IdBotaoNovo = 1;
    private const int IdBotaoJogo = 2;

    private AnimadorSelecao animadorSelecao;
    private bool jogadorPodeInteragir = true;
    private bool jogadorSelecionouUmaOpcao;
    private bool fecharJogo;

    private int ultimoBotao;
    private int penultimoBotao;
    private int clickCount;

    private bool dentroDoTempoLimiteParaClicarEmJogo = false;

    // Use this for initialization
    void Start()
    {
        animadorSelecao = GetComponent<AnimadorSelecao>();

        StartCoroutine(DarDicaACadaXSegundos());
    }

    private IEnumerator DarDicaACadaXSegundos()
    {
        yield return new WaitForSeconds(2);
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

        StartCoroutine(ContagemCliqueJogo());
    }

    private IEnumerator ContagemCliqueJogo()
    {
        dentroDoTempoLimiteParaClicarEmJogo = true;
        yield return new WaitForSeconds(TempoParaNovoJogo);
        dentroDoTempoLimiteParaClicarEmJogo = false;
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

        animadorSelecao.AnimarSelecao(BotaoJogo);

        if (!dentroDoTempoLimiteParaClicarEmJogo)
            return;

        Clicar(IdBotaoJogo);
        VerificarCarregamentoDoMapa();
    }

    void BotaoSairClick()
    {
        if (!jogadorPodeInteragir)
            return;

        animadorSelecao.AnimarSelecao(BotaoSair);

        jogadorPodeInteragir = false;
        StartCoroutine(FecharEmAlgunsSegundos(1f));
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
        return clickCount >= 2;
    }

    private IEnumerator CarregarCenario()
    {
        CarregandoTextMesh.SetActive(true);
        yield return new WaitForSeconds(animadorSelecao.DuracaoAnimacao + .5f);
        Application.LoadLevel(NomeLevelNovoJogo);
    }

    private IEnumerator FecharEmAlgunsSegundos(float tempo)
    {
        Debug.Log("saindo");
        yield return new WaitForSeconds(tempo);
        Application.Quit();
    }
}
