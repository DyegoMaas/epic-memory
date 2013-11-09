using UnityEngine;
using System.Collections;

public class CarregamentoObjetos : MonoBehaviour
{
    public GameObject MessageManager;
    public string NomeLevelCarregar = "jogo";

    public tk2dTextMesh PercentualCarregamento;

    protected int PercentualCarregado
    {
        get { return (int)(Application.GetStreamProgressForLevel(NomeLevelCarregar) * 100f); }
    }

    // Use this for initialization
    void Start ()
	{
	    CarregarObjetos();
        Application.LoadLevel(NomeLevelCarregar);
	}

    // Update is called once per frame
    void Update ()
    {
        AtualizarPercentualCarregamento();
    }

    private void AtualizarPercentualCarregamento()
    {
        if (PercentualCarregamento)
        {
            PercentualCarregamento.text = string.Format("{0}%", PercentualCarregado);
            PercentualCarregamento.Commit();
        }
    }

    private void CarregarObjetos()
    {
        if(MessageManager)
            CarregarMessageManager(MessageManager);
    }

    private void CarregarMessageManager(GameObject prefab)
    {
        Instantiate(prefab, new Vector3(100, 0, 0), new Quaternion());
    }
}
