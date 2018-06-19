using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    List<Enemy> enemies;

    public List<Enemy> Enemies { get { return enemies; } }

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
}
