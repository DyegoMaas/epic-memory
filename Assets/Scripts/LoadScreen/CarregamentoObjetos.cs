using UnityEngine;
using System.Collections;

public class CarregamentoObjetos : MonoBehaviour
{
    public GameObject MessageManager;
    public string NomeLevelCarregar = "jogo";
    public bool CarregarLevel = true;

    public tk2dTextMesh PercentualCarregamento;

    protected int PercentualCarregado
    {
        get { return (int)(Application.GetStreamProgressForLevel(NomeLevelCarregar) * 100f); }
    }

    // Use this for initialization
    void Start ()
	{
	    CarregarObjetos();

        if(CarregarLevel)
            Application.LoadLevel(NomeLevelCarregar);
	}

    private void CarregarObjetos()
    {
        if(MessageManager)
            Instantiate(MessageManager, new Vector3(100, 0, 0), new Quaternion());
    }

    // Update is called once per frame
    void Update ()
    {
        AtualizarPercentualCarregamento();
    }

    private void AtualizarPercentualCarregamento()
    {
        if (!PercentualCarregamento)
            return;
        
        string novoPercentual = string.Format("{0}%", PercentualCarregado);
        if (novoPercentual != PercentualCarregamento.text)
        {
            Debug.Log(novoPercentual);
            PercentualCarregamento.text = novoPercentual;
            PercentualCarregamento.Commit();
        }
    }
}
