using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacter))]
[RequireComponent(typeof(NavMeshObstacle))]
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
    NavMeshAgent agent;
    ThirdPersonCharacter character;
    NavMeshObstacle obstacle;

    bool alive;

    public List<Renderer> defaultRenderers;
    public List<Renderer> irRenderers;

    public Transform currentGoal;
    public float arrivalDistance;

    [SerializeField]
    float stepModifier = 1;

    //TODO create AIController class to manage movement animations and navmeshagent stuff

	// Use this for initialization
	void Start () {
        GameManager.instance.OnEnemySpawn(this);
        health = maxHealth;

        healthBar.maxHealth = maxHealth;
        healthBar.health = health;

        m_animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        character = GetComponent<ThirdPersonCharacter>();

        agent.updateRotation = false;
        agent.updatePosition = true;

        obstacle.enabled = false;

        alive = true;

        currentGoal = GameManager.instance.GetRandomGoal(null);
        agent.SetDestination(currentGoal.position);

    }
	
	// Update is called once per frame
	void Update () {
        infoCanvas.transform.forward = Camera.main.transform.forward;

        if (alive)
        {
            if(agent.pathStatus != NavMeshPathStatus.PathComplete || Vector3.Distance(transform.position, currentGoal.position) < arrivalDistance)
            {
                currentGoal = GameManager.instance.GetRandomGoal(currentGoal);
                agent.SetDestination(currentGoal.position);
            }
            if(agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity / stepModifier, false, false);
            } else
            {
                character.Move(Vector3.zero, false, false);
            }
        }
        else
        {

            character.Move(Vector3.zero, false, false);
            // fade away and despawn when healthbar animation finishes
            if (healthBar.DeathFinished)
            {
                m_animator.SetTrigger("Fade");
                GameObject.Destroy(this.gameObject, 5);
            }
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
        agent.enabled = false;
        obstacle.enabled = true;
        currentGoal = null;
        health = 0;
        alive = false;
    }
}
