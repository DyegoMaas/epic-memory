using UnityEngine;
using System.Collections;

public class Personagem : MonoBehaviour {

    public int Id { get; private set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DefinirId(int id)
    {
        Id = id;
        gameObject.name += "_" + id.ToString().PadLeft(2, '0');
    }
}
