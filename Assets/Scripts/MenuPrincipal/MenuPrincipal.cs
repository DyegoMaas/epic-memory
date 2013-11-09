using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimadorSelecao))]
public class MenuPrincipal : MonoBehaviour
{
    public string NomeLevelNovoJogo = "load_sceen";
    public GameObject BotaoNovo;
    public GameObject BotaoJogo;

    private const int IdBotaoNovo = 1;
    private const int IdBotaoJogo = 2;

    private readonly Stack<int> clicks = new Stack<int>();
    private AnimadorSelecao animadorSelecao;

    // Use this for initialization
	void Start ()
	{
	    animadorSelecao = GetComponent<AnimadorSelecao>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void BotaoNovoClick()
    {
        clicks.Push(IdBotaoNovo);
        animadorSelecao.AnimarSelecao(BotaoNovo);
    }

    void BotaoJogoClick()
    {
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
            StartCoroutine(CarregarCenario());
        }
    }

    private IEnumerator CarregarCenario()
    {
        yield return new WaitForSeconds(animadorSelecao.DuracaoAnimacao + .5f);
        Application.LoadLevel(NomeLevelNovoJogo);
    }
}
