using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Spawns particle effect and optional audio when shot
public class HitParticleSpawner : MonoBehaviour {

    public enum DirectionRule
    {
        normal,         // Effect oriented to normal
        reflect,        // Effect is ray reflected by normal
        rayDirection,   // Effect oriented to incoming ray
        up              // Effect oriented up

    }

	[SerializeField]
	ParticleSystem particles;

    [SerializeField]
    float offset = 0;   // distance from point of contact to spawn effect

    [SerializeField]
    float duration = 5; // Time until effect destroyed

    [SerializeField]
    DirectionRule directionRule = DirectionRule.normal;

    [SerializeField]
    bool playAudio;

    [SerializeField]
    AudioClip hitAudio;

    [SerializeField]
    float audioVolume = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Shoot(RaycastHit hit, Ray ray){
		Vector3 normal = hit.normal;
        Vector3 position = hit.point + normal * offset;
        // Orient effect based on rul
        Vector3 direction = new Vector3();
        switch (directionRule)
        {
            case DirectionRule.normal:
                direction = normal;
                break;
            case DirectionRule.reflect:
                direction = Vector3.Reflect(ray.direction, normal);
                break;
            case DirectionRule.rayDirection:
                direction = ray.direction;
                break;
            case DirectionRule.up:
                direction = Vector3.up;
                break;
        }

        // Spawn effect at given position and orientation
        GameObject particleClone = GameObject.Instantiate(particles.gameObject,position, Quaternion.LookRotation(direction));
		ParticleSystem effect = particleClone.GetComponent<ParticleSystem> ();
		effect.Play ();

        if(duration > 0)
        {
            GameObject.Destroy(particleClone, duration);
        }

        // Play audio
        if(playAudio && hitAudio != null)
        {
            AudioSource.PlayClipAtPoint(hitAudio, position, audioVolume);
        }
	}
}
