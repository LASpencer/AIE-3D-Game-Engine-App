using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    List<Enemy> enemies;

    public List<Enemy> Enemies { get { return enemies; } }

    public List<Transform> Goals;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);

        enemies = new List<Enemy>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Cancel"))
        {
            Application.Quit();
        }
	}

    void OnSceneChange()
    {

    }

    public void OnEnemySpawn(Enemy enemy)
    {
        enemies.Add(enemy);
    }

    public void OnEnemyDestroyed(Enemy enemy)
    {
        enemies.Remove(enemy);
        Debug.Log("Enemy destroyed");
    }

    public Transform GetRandomGoal(Transform lastPicked)
    {
        //HACK
        Transform newGoal = null;
        while (newGoal == null)
        {
            int index = Random.Range(0, Goals.Count);
            if(lastPicked == null || Goals[index] != lastPicked)
            {
                newGoal = Goals[index];
            }
        }
        return newGoal;
    }
}
