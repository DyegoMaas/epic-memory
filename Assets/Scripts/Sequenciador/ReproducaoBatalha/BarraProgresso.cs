using UnityEngine;

public class BarraProgresso : InjectionBehaviour
{
    public float EscalaFinal;

    [InjectedDependency] private IProgressaoBatalha progressaoBatalha;

    private float percentual;

    protected override void StartOverride()
    {
    }

    // Update is called once per frame
	void Update ()
	{
        if (percentual != progressaoBatalha.PercentualCompleto)
        {
            percentual = progressaoBatalha.PercentualCompleto;
            AnimarEscala();
        }
	}

    private void AnimarEscala()
    {
        var novaEscala = new Vector3(CalcularEscala(), 1f, 1f);
        iTween.ScaleTo(gameObject, iTween.Hash("scale", novaEscala, "time", .3f, "easetype", iTween.EaseType.easeOutCubic, "islocal", true));
    }

    private float CalcularEscala()
    {
        return progressaoBatalha.PercentualCompleto * EscalaFinal;
    }
}
