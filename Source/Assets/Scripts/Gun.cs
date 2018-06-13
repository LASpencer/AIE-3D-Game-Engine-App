using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    Animator anim;
    AudioSource audio;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        audio = GetComponentInChildren<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool Fire(){
        //TODO make gun fire (particle effect + sound effect, animation)

        anim.SetTrigger("Shoot");
        audio.Play();

        return true;
	}

	public bool Reload(){
		//TODO reload gun
		return true;
	}
}
