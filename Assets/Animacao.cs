using UnityEngine;
using System.Collections;

public class Animacao : MonoBehaviour {

    public GameObject prefabHit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void AnimacaoHit()
    {
        if (prefabHit)
        {
            var hit = Instantiate(prefabHit, transform.position, transform.rotation);
            
            Debug.Log("Criado hit em " + transform.position);
        }
    }
}
