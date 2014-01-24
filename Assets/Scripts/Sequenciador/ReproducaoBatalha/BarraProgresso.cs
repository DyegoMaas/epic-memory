using UnityEngine;

public class BarraProgresso : InjectionBehaviour
{
    public float EscalaFinal = .01f;

    [InjectedDependency] private IProgressaoBatalha progressaoBatalha;

    private float percentual;

    protected override void StartOverride()
    {
    }

    // Update is called once per frame
	void Update ()
	{
        if (percentual > progressaoBatalha.PercentualCompleto || percentual < progressaoBatalha.PercentualCompleto)
        {
            percentual = progressaoBatalha.PercentualCompleto;
            AnimarEscala();
        }
	}

    private void AnimarEscala()
    {
        iTween.ScaleTo(gameObject, iTween.Hash("scale", CalcularNovaEscala(), "time", .3f, "easetype", 
            iTween.EaseType.easeOutCubic, "islocal", true));
    }

    private Vector3 CalcularNovaEscala()
    {
        return new Vector3(progressaoBatalha.PercentualCompleto * EscalaFinal, 1f, 1f);
    }
}
