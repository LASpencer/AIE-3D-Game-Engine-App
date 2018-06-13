using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

	// TODO interact mode

	[SerializeField]
	Gun gun;
	[SerializeField]
	LayerMask shootMask;
    [SerializeField]
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
            Ray shootRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            //TODO figure out why gun not hitting (ray is wrong?)
            if (canShoot && Physics.Raycast (shootRay, out gunHit, 100, shootMask)) {
				// Get 
				HitParticleSpawner particleSpawner = gunHit.collider.GetComponent<HitParticleSpawner>();
                if (particleSpawner != null)
                {
                    particleSpawner.Shoot(gunHit, shootRay);
                }
			}
		}
	}
}
