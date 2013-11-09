using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class MenuPrincipal : MonoBehaviour {

    private Stack<int> clicks = new Stack<int>();

    private const int IdBotaoNovo = 1;
    private const int IdBotaoJogo = 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void BotaoNovoClick()
    {
        clicks.Push(IdBotaoNovo);
    }

    void BotaoJogoClick()
    {
        clicks.Push(IdBotaoJogo);
        
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
            Application.LoadLevel("jogo");
        }
    }
}
