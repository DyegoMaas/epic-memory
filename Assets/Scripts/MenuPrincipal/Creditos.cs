using UnityEngine;
using System.Collections;

public class Creditos : MonoBehaviour
{
    private float positcaoInicalY;
    private bool visivel;

    // Use this for initialization
    void Start()
    {
        positcaoInicalY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AbrirClick()
    {
        if (!visivel)
        {
            Show();
            visivel = true;
        }
    }

    void FecharClick()
    {
        if (visivel)
        {
            Hide();
            visivel = false;
        }
    }

    private void Hide()
    {
        var posicaoFinal = new Vector3(transform.position.x, positcaoInicalY, transform.position.z);
        iTween.MoveTo(gameObject, iTween.Hash("position", posicaoFinal, "time", .4f, "easetype", iTween.EaseType.easeOutCirc, "islocal", true));
    }

    private void Show()
    {
        var posicaoFinal = new Vector3(transform.position.x, 0, transform.position.z);
        iTween.MoveTo(gameObject, iTween.Hash("position", posicaoFinal, "time", .4f, "easetype", iTween.EaseType.easeOutCirc, "islocal", true));
    }
}
