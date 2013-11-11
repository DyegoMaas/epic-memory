using System;
using UnityEngine;
using System.Collections;

public class ToggleButtonDificuldade : MonoBehaviour
{
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnToggleState(tk2dUIMultiStateToggleButton button)
    {
        Dificuldade dificuldade = MapearDificuldade(button.Index);
        SendMessageUpwards("EscolherDificuldade", dificuldade);
    }

    private Dificuldade MapearDificuldade(int index)
    {
        return (Dificuldade)(index + 1);
    }
}
