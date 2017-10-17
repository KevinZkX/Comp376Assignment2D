using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public void DestroyObject()
    {
        Animator anim = GetComponent<Animator>();
       
       
            if (anim != null)
            {
                anim.enabled = false;
            }
            Destroy(gameObject);
       
    }
}
