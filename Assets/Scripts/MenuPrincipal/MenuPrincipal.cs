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

    private AnimadorSelecao animadorSelecao;
    private bool jogadorPodeInteragir = true;

    // Use this for initialization
    void Start()
    {
        animadorSelecao = GetComponent<AnimadorSelecao>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void BotaoNovoClick()
    {
        if (!jogadorPodeInteragir)
            return;

        animadorSelecao.AnimarSelecao(BotaoNovo);
        
        jogadorPodeInteragir = false;
        StartCoroutine(CarregarCenario());
    }

    private IEnumerator CarregarCenario()
    {
        CarregandoTextMesh.SetActive(true);
        yield return new WaitForSeconds(animadorSelecao.DuracaoAnimacao + .5f);
        Application.LoadLevel(NomeLevelNovoJogo);
    }

    void BotaoSairClick()
    {
        if (!jogadorPodeInteragir)
            return;

        animadorSelecao.AnimarSelecao(BotaoSair);

        jogadorPodeInteragir = false;
        StartCoroutine(FecharEmAlgunsSegundos(1f));
    }

    private IEnumerator FecharEmAlgunsSegundos(float tempo)
    {
        Debug.Log("saindo");
        yield return new WaitForSeconds(tempo);
        Application.Quit();
    }
}
