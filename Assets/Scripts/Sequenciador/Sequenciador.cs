using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequenciador : MonoBehaviour
{
    public static Sequenciador Instancia;

    public Camera Camera;
    public Personagem[] TimeA;
    public Personagem[] TimeB;
    public int DuracaoAtaque = 1;

    private GeradorAtaques geradorAtaques;
    private readonly List<Ataque> ataques = new List<Ataque>();

    void Awake()
    {
        Instancia = this;
    }

    // Use this for initialization
	void Start ()
	{
	    var arena = new Arena();

	    foreach (var personagem in TimeA)
	    {
            int idPersonagem = arena.AdicionarParticipanteAoTimeA();    
            personagem.DefinirId(idPersonagem);
	    }

        foreach (var personagem in TimeB)
        {
            int idPersonagem = arena.AdicionarParticipanteAoTimeB();
            personagem.DefinirId(idPersonagem);
        }

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
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                foreach (var personagem in TimeA)
                {
                    if (PersonagemFoiSelecionado(hit, personagem))
                    {
                        Debug.Log(string.Format("{0} selecionado", personagem.gameObject.name));
                        continue;
                    }
                }

                foreach (var personagem in TimeB)
                {
                    if (PersonagemFoiSelecionado(hit, personagem))
                    {
                        Debug.Log(string.Format("{0} selecionado", personagem.gameObject.name));
                    }
                }
            }
        }
	}

    private static bool PersonagemFoiSelecionado(RaycastHit hit, Personagem personagem)
    {
        return personagem.collider.transform == hit.transform;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "Gerar ataque"))
        {
            var ataque = geradorAtaques.GerarAtaque();
            ataques.Add(ataque);
            Debug.Log(ataque);
        }

        if (GUI.Button(new Rect(0, 100, 100, 50), "Reproduzir Sequência"))
        {
            StartCoroutine(ReproduzirSequenciaAtaques());
        }
    }

    private IEnumerator ReproduzirSequenciaAtaques()
    {
        Debug.Log("-----");
        Debug.Log("Sequência de ataques:");
        foreach (var ataque in ataques)
        {
            ReproduzirAtaque(ataque);
            yield return new WaitForSeconds(DuracaoAtaque);
        }
    }

    private void ReproduzirAtaque(Ataque ataque)
    {
        Debug.Log(ataque);
    }
}
