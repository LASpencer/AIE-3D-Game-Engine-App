using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticleSpawner : MonoBehaviour {

	[SerializeField]
	ParticleSystem particles;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Shoot(RaycastHit hit){
		Vector3 position = hit.point;
		Vector3 normal = hit.normal;
		// TODO spawn particles at point
		GameObject particleClone = GameObject.Instantiate(particles.gameObject,position,Quaternion.LookRotation(normal));
		ParticleSystem effect = particleClone.GetComponent<ParticleSystem> ();
		effect.Play ();
	}
}
