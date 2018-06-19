using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

	[SerializeField]
	Gun gun;
	[SerializeField]
	LayerMask shootMask;
    [SerializeField]
    LayerMask interactMask;
    [SerializeField]
	Camera playerCamera;

    VisorController visor;

    [SerializeField]
    float interactRange;

	// Use this for initialization
	void Start () {
		playerCamera = GetComponentInChildren<Camera> ();
        visor = GetComponent<VisorController>();
	}
	
	// Update is called once per frame
	void Update () {
        //TODO check if player looking at interactable, if so select it. Also, if use button pressed, do it's action
        RaycastHit interactHit;
        Ray shootRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(shootRay, out interactHit, interactRange, interactMask, QueryTriggerInteraction.Collide))
        {
            Interactable target = interactHit.collider.GetComponent<Interactable>();
            if (target != null)
            {
                target.Select();

                if (Input.GetButtonDown("Interact"))
                {
                    target.Interact();
                }
            }
        }


        if (Input.GetButtonDown("Fire"))
        {
            bool canShoot = gun.Fire();
            RaycastHit gunHit;
            //TODO figure out why gun not hitting (ray is wrong?)
            if (canShoot)
            {
                // Shooting disables visor
                visor.SetVisor(false);

                Debug.DrawRay(shootRay.origin, shootRay.direction,Color.white,1);

                if (Physics.Raycast(shootRay, out gunHit, 100, shootMask))
                {
                    // Get 
                    HitParticleSpawner particleSpawner = gunHit.collider.GetComponent<HitParticleSpawner>();
                    Enemy enemy = gunHit.collider.GetComponentInParent<Enemy>();
                    if (particleSpawner != null)
                    {
                        particleSpawner.Shoot(gunHit, shootRay);
                    }
                    if(enemy != null)
                    {
                        bool headshot = gunHit.collider.CompareTag("Head");
                        float damage = gun.damage;
                        if (headshot)
                        {
                            damage *= gun.headshotBonus;
                        }
                        enemy.InflictDamage(damage);
                    }
                }
            }
        }
	}
}
