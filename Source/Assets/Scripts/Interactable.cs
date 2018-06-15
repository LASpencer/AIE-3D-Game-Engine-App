using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Interactable : MonoBehaviour {

    public string verb;
    public UnityEvent OnInteract;
	public UnityEvent OnSelect;
	public UnityEvent OnDeselect;

    [SerializeField]
    Text verbText;
    [SerializeField]
    Canvas displayCanvas;

	bool wasSelected;	// Was it selected the previous frame?
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
			if (!wasSelected ()) {
				OnSelect.Invoke ();
			}
			wasSelected = true;
            //TODO put selected effect on object
        } else
        {
            displayCanvas.gameObject.SetActive(false);
			if (wasSelected) {
				OnDeselect.Invoke ();
			}
			wasSelected = false;
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
