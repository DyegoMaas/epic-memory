using UnityEngine;

public class GerenciadorDificuldade
{
    public Dificuldade DificuldadeSelecionada { get; private set; }

    public void EscolherDificuldade(Dificuldade novaDificuldade)
    {
        DificuldadeSelecionada = novaDificuldade;
        Debug.Log(novaDificuldade.ToString());
    }

    /// <summary>
    /// 0 == impossível
    /// 1 == fácil
    /// </summary>
    public float CoeficienteFacilidade
    {
        get
        {
            if (DificuldadeSelecionada == Dificuldade.Facil)
                return 1f;
            return .5f;
        }
    }
}