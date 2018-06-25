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
    Canvas infoCanvas;  // Worldspace UI canvas for enemy

    [SerializeField]
    float maxHealth = 100;

    [SerializeField]
    float health;

    [SerializeField]
    HealthBar healthBar;

    Animator m_animator;
    NavMeshAgent agent;
    ThirdPersonCharacter character;
    NavMeshObstacle obstacle;           // obstacle for corpse

    bool alive;

    public List<Renderer> defaultRenderers;     // Renderers on Default layer
    public List<Renderer> irRenderers;          // Renderers with IR material on Enemy layer

    public Transform currentGoal;
    public float arrivalDistance;

    [SerializeField]
    float stepModifier = 1;

	// Use this for initialization
	void Start () {
        GameManager.instance.OnEnemySpawn(this);

        health = maxHealth;
        healthBar.maxHealth = maxHealth;
        healthBar.health = health;
        alive = true;

        m_animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        character = GetComponent<ThirdPersonCharacter>();

        agent.updateRotation = false;
        agent.updatePosition = true;

        obstacle.enabled = false;

        currentGoal = GameManager.instance.GetRandomGoal(null);
        agent.SetDestination(currentGoal.position);

    }
	
	// Update is called once per frame
	void Update () {
        infoCanvas.transform.forward = Camera.main.transform.forward;

        if (alive)
        {
            // Get new goal if path blocked or goal reached
            if(agent.pathStatus != NavMeshPathStatus.PathComplete || Vector3.Distance(transform.position, currentGoal.position) < arrivalDistance)
            {
                currentGoal = GameManager.instance.GetRandomGoal(currentGoal);
                agent.SetDestination(currentGoal.position);
            }
            // Move towards current goal
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

    // Stop moving and become an obstacle
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
