using UnityEngine;
using System.Collections;

public class MenuDificuldade : MonoBehaviour {

    private bool expandido = false;

    public Vector3 PosicaoExpandido;
    public Vector3 PosicaoRetraido;

    public iTween.EaseType EaseType = iTween.EaseType.easeOutBack;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ToggleState()
    {
        Debug.Log("dificuldade");
        expandido = !expandido;

        if (expandido)
            SubirMenu();
        else
            DescerMenu();
    }

    private void SubirMenu()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", PosicaoExpandido, "time", .4f, "easetype", EaseType, "islocal", true));
    }

    private void DescerMenu()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", PosicaoRetraido, "time", .4f, "easetype", EaseType, "islocal", true));
    }
}
