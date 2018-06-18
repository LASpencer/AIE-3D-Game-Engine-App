using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    List<MaterialSwapper> materialSwappers;

    [SerializeField]
    Canvas infoCanvas;

	// Use this for initialization
	void Start () {
        GameManager.instance.OnEnemySpawn(this);
        materialSwappers = new List<MaterialSwapper>(GetComponentsInChildren<MaterialSwapper>());
	}
	
	// Update is called once per frame
	void Update () {
        infoCanvas.transform.forward = Camera.main.transform.forward;
	}

    private void OnDestroy()
    {
        GameManager.instance.OnEnemyDestroyed(this);
    }

    public void SetVisor(bool visorOn)
    {
        if (visorOn)
        {
            foreach(MaterialSwapper m in materialSwappers)
            {
                m.setMaterial(1);
            } 
        }
        else
        {
            foreach(MaterialSwapper m in materialSwappers)
            {
                m.setMaterial(0);
            }
        }
    }
}
