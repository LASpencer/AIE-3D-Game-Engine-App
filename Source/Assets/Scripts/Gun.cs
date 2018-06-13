using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    Animator anim;
    AudioSource audio;
    ParticleSystem gunsmoke;

    [SerializeField]
    GameObject shell;

    [SerializeField]
    float rof = 0.5f;

    float shotCD = 0;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        audio = GetComponentInChildren<AudioSource>();
        gunsmoke = GetComponentInChildren<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update () {
        shotCD = Mathf.Max(shotCD - Time.deltaTime, 0);
	}

	public bool Fire(){
        //TODO make gun fire (particle effect + sound effect, animation)
        if (shotCD == 0)
        {
            shotCD = rof;
            anim.SetTrigger("Shoot");
            audio.Play();
            gunsmoke.Play();
            return true;
        } else
        {
            return false;
        }
	}

	public bool Reload(){
		//TODO reload gun
		return true;
	}

    //TODO based on animation event, spawn bullet casing
}
