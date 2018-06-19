using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour {

    [SerializeField]
    Canvas infoCanvas;

    [SerializeField]
    float maxHealth = 100;

    [SerializeField]
    float health;

    [SerializeField]
    HealthBar healthBar;

    Animator m_animator;

    public List<Renderer> defaultRenderers;
    public List<Renderer> irRenderers;

    //TODO create AIController class to manage movement animations and navmeshagent stuff

	// Use this for initialization
	void Start () {
        GameManager.instance.OnEnemySpawn(this);
        health = maxHealth;

        healthBar.maxHealth = maxHealth;
        healthBar.health = health;

        m_animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        infoCanvas.transform.forward = Camera.main.transform.forward;

        //TODO handle being killed
        if(health <= 0 && healthBar.DeathFinished)
        {
            m_animator.SetTrigger("Fade");
            //TODO make Fade state (and state machine behaviour) causing them to fade out
            //HACK figure out if there's a better way to do this?
            GameObject.Destroy(this.gameObject, 5);
        }
	}

    private void OnDestroy()
    {
        GameManager.instance.OnEnemyDestroyed(this);
    }

    public void InflictDamage(float damage)
    {
        if(health > 0)
        {
            healthBar.InflictDamage(damage);
            health -= damage;
            if(health <= 0)
            {
                Kill();
            }
        }
    }

    void Kill()
    {
        m_animator.SetTrigger("Dead");
        health = 0;
    }
}
