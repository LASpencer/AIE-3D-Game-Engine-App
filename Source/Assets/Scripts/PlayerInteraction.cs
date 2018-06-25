using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

	[SerializeField]
	Gun gun;
	[SerializeField]
	LayerMask shootMask;        // Mask for weapon raycast
    [SerializeField]
    LayerMask interactMask;     // Mask for object picking raycast
    [SerializeField]
	Camera playerCamera;

    VisorController visor;      // Activate or deactivate visor effect

    [SerializeField]
    float interactRange;

	// Use this for initialization
	void Start () {
		playerCamera = GetComponentInChildren<Camera> ();
        visor = GetComponent<VisorController>();
	}
	
	// Update is called once per frame
	void Update () {
        // Check if player is looking at interactable
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

        // Check if player is shooting
        if (Input.GetButtonDown("Fire"))
        {
            bool canShoot = gun.Fire();
            RaycastHit gunHit;
            if (canShoot)
            {
                // Shooting disables visor
                visor.SetVisor(false);

                if (Physics.Raycast(shootRay, out gunHit, 100, shootMask,QueryTriggerInteraction.Collide))
                {
                    // Probably better to have some target component with an invokable event instead
                    HitParticleSpawner particleSpawner = gunHit.collider.GetComponent<HitParticleSpawner>();
                    Enemy enemy = gunHit.collider.GetComponentInParent<Enemy>();
                    Destroyable destroyable = gunHit.collider.GetComponent<Destroyable>();

                    // Spawn particles
                    if (particleSpawner != null)
                    {
                        particleSpawner.Shoot(gunHit, shootRay);
                    }

                    // Damage enemy
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

                    // Destroy object
                    if(destroyable != null)
                    {
                        destroyable.Shoot(gun.damage);
                    }
                }
            }
        }
	}
}
