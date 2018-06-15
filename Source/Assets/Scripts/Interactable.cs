using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour {

    public string verb;
    public UnityEvent OnInteract;

    [SerializeField]
    Text verbText;
    [SerializeField]
    Canvas displayCanvas;

    bool selected;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (selected)
        {
            displayCanvas.gameObject.SetActive(true);
            verbText.text = verb;

            //TODO put selected effect on object
        } else
        {
            displayCanvas.gameObject.SetActive(false);
        }

        selected = false;
	}

    public void Select()
    {
        selected = true;
    }

    public void Interact()
    {
        OnInteract.Invoke();
    }
}
