﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

	// TODO interact mode

	[SerializeField]
	Gun gun;
	[SerializeField]
	LayerMask shootMask;
	Camera playerCamera;

	// Use this for initialization
	void Start () {
		playerCamera = GetComponentInChildren<Camera> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1")){
			bool canShoot = gun.Fire();
			RaycastHit gunHit;
			//TODO figure out why gun not hitting (ray is wrong?)
			if (canShoot && Physics.Raycast (playerCamera.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 1)), out gunHit, shootMask)) {
				// Get 
				HitParticleSpawner particleSpawner = gunHit.collider.GetComponent<HitParticleSpawner>();
			}
		}
	}
}
