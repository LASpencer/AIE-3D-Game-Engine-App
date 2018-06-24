using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticleSpawner : MonoBehaviour {

    public enum DirectionRule
    {
        normal,
        reflect
    }

	[SerializeField]
	ParticleSystem particles;

    [SerializeField]
    float offset = 0;

    [SerializeField]
    float duration = 5;

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
        Vector3 direction = new Vector3();
        switch (directionRule)
        {
            case DirectionRule.normal:
                direction = normal;
                break;
            case DirectionRule.reflect:
                direction = Vector3.Reflect(ray.direction, normal);
                break;
        }

        // TODO spawn particles at point
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
