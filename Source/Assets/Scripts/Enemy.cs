using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    Canvas infoCanvas;

    [SerializeField]
    float maxHealth = 100;

    [SerializeField]
    float health;

    [SerializeField]
    HealthBar healthBar;

	// Use this for initialization
	void Start () {
        GameManager.instance.OnEnemySpawn(this);
        health = maxHealth;

        healthBar.maxHealth = maxHealth;
        healthBar.health = health;
	}
	
	// Update is called once per frame
	void Update () {
        infoCanvas.transform.forward = Camera.main.transform.forward;

        //HACK
        if (Input.GetKeyDown(KeyCode.K))
        {
            healthBar.InflictDamage(10);
        }
	}

    private void OnDestroy()
    {
        GameManager.instance.OnEnemyDestroyed(this);
    }
}
