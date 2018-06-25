using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Opens and closes when interactable used
[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(MeshRenderer))]
public class Gate : MonoBehaviour {

	Interactable gateInteraction;
	Animator gateAnimation;
	MeshRenderer gateRenderer;
	[SerializeField]
	bool isOpen;
	[SerializeField]
	Material regularMaterial;
	[SerializeField]
	Material highlightMaterial;

	// Use this for initialization
	void Start () {
		gateInteraction = GetComponent<Interactable> ();
		gateAnimation = GetComponent<Animator> ();
		gateRenderer = GetComponent<MeshRenderer> ();

		if (isOpen) {
			gateInteraction.verb = "Close";
		} else {
			gateInteraction.verb = "Open";
		}

		gateInteraction.OnInteract.AddListener (toggleGate);
		gateInteraction.OnSelect.AddListener (SelectGate);
		gateInteraction.OnDeselect.AddListener (DeselectGate);

		gateAnimation.SetBool ("Open", isOpen);
		gateRenderer.material = regularMaterial;
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

	void SelectGate(){
		gateRenderer.material = highlightMaterial;
	}

	void DeselectGate(){
		gateRenderer.material = regularMaterial;
	}
}
