using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Animator))]
public class Gate : MonoBehaviour {

	Interactable gateInteraction;
	Animator gateAnimation;
	[SerializeField]
	bool isOpen;


	// Use this for initialization
	void Start () {
		gateInteraction = GetComponent<Interactable> ();
		gateAnimation = GetComponent<Animator> ();


		if (isOpen) {
			gateInteraction.verb = "Close";
		} else {
			gateInteraction.verb = "Open";
		}

		gateInteraction.OnInteract.AddListener (toggleGate);

		gateAnimation.SetBool ("Open", isOpen);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void toggleGate(){
		isOpen = !isOpen;
		if (isOpen) {
			gateInteraction.verb = "Close";
		} else {
			gateInteraction.verb = "Open";
		}
		gateAnimation.SetBool ("Open", isOpen);
	}
}
