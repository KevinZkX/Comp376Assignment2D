using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DestroyObject()
    {
        Debug.Log("Explo");
        Animation anim = GetComponent<Animation>();
        Debug.Log(anim.isPlaying);
        anim.enabled = true;
        //Destroy(gameObject);
    }
}
