using UnityEngine;
using System.Collections;

public class AnimadorSelecao : MonoBehaviour
{

    public float DuracaoAnimacao = .5f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AnimarSelecao()
    {
        StartCoroutine(Animar(gameObject));
    }

    public void AnimarSelecao(GameObject objetoAnimar)
    {
        StartCoroutine(Animar(objetoAnimar));
    }

    private IEnumerator Animar(GameObject objetoAnimar)
    {
        var escalaOriginal = objetoAnimar.transform.localScale;
        var escalaFinal = objetoAnimar.transform.localScale * 1.5f;

        iTween.ScaleTo(objetoAnimar, escalaFinal, TempoAnimacao());
        yield return new WaitForSeconds(TempoPausa());
        iTween.ScaleTo(objetoAnimar, escalaOriginal, TempoAnimacao());
    }

    private float TempoAnimacao()
    {
        return DuracaoAnimacao * .4f;
    }

    private float TempoPausa()
    {
        return DuracaoAnimacao * .2f;
    }
}
