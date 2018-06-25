using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This component destoys the object when shot
[RequireComponent(typeof(Collider))]
public class Destroyable : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot(float damage)
    {
        GameObject.Destroy(this.gameObject);
    }
}
