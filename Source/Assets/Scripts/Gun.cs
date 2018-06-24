using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    Animator anim;
    AudioSource audio;
    ParticleSystem gunsmoke;


    [Header("Combat Stats")]
    [SerializeField]
    float rof = 0.5f;

    float shotCD = 0;

    public float damage = 20;
    public float headshotBonus = 2;

    [Header("Shell Ejection")]
    [SerializeField]
    GameObject shell;

    [SerializeField]
    Transform shellPosition;

    [SerializeField]
    float ejectionSpeed;


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
    public void EjectShell()
    {
        GameObject ejected = Instantiate(shell, shellPosition.position, shellPosition.rotation);
        Rigidbody shellBody = ejected.GetComponent<Rigidbody>();
        if(shellBody != null)
        {
            shellBody.velocity = shellPosition.right * ejectionSpeed;
        }
        GameObject.Destroy(ejected, 10);
    }
}
