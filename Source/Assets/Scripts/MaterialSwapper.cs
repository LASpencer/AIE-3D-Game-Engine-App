using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialSwapper : MonoBehaviour {

    Renderer renderer;

    public List<Material> materials;

	// Use this for initialization
	void Start () {
        renderer = GetComponent<Renderer>();
        renderer.material = materials[0];
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setMaterial(int index)
    {
        renderer.material = materials[index];
    }
}
