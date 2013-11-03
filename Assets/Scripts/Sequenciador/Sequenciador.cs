using System.Collections.Generic;
using UnityEngine;

public class Sequenciador : MonoBehaviour
{
    private GeradorAtaques geradorAtaques;
    private readonly List<Ataque> ataques = new List<Ataque>();

    // Use this for initialization
	void Start ()
	{
	    var arena = new Arena();
	    arena.AdicionarParticipanteAoTimeA();
	    arena.AdicionarParticipanteAoTimeA();
	    arena.AdicionarParticipanteAoTimeB();
	    arena.AdicionarParticipanteAoTimeB();

	    ImprimirTime('A', arena.TimeA);
	    ImprimirTime('B', arena.TimeB);

	    geradorAtaques = new GeradorAtaques(arena, new UnityRandomizer());
	}
    
    private void ImprimirTime(char sigla, IEnumerable<int> time)
    {
        Debug.Log(string.Format("Time {0}:", sigla));
        foreach (var id in time)
        {
            Debug.Log(id);
        }
    }

    // Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 50, 50), "Gerar ataque"))
        {
            var ataque = geradorAtaques.GerarAtaque();
            ataques.Add(ataque);
            Debug.Log(ataque);
        }
    }
}
