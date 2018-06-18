using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisorController : MonoBehaviour {

    [SerializeField]
    PostProcessing mainCamera;
    [SerializeField]
    PostProcessing UICamera;
    [SerializeField]
    PostProcessing gunCamera;
    [SerializeField]
    PostProcessing enemyCamera;

    bool visorOn;    //TODO use animations to smoothly change instead

    bool VisorOn { get { return visorOn; } }

    // Use this for initialization
    void Start () {
        visorOn = false;
        mainCamera.drawEdges = false;
        gunCamera.drawEdges = false;
        gunCamera.drawVisor = false;
        enemyCamera.gameObject.SetActive(false);

        //TODO change material on all enemies?
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Visor"))
        {
            SetVisor(!visorOn);
        }
	}

    public void SetVisor(bool value)
    {
        visorOn = value;
        if (visorOn)
        {
            mainCamera.drawEdges = true;
            gunCamera.drawEdges = true;
            gunCamera.drawVisor = true;
            enemyCamera.gameObject.SetActive(true);
        }
        else
        {
            mainCamera.drawEdges = false;
            gunCamera.drawEdges = false;
            gunCamera.drawVisor = false;
            enemyCamera.gameObject.SetActive(false);
        }
        foreach(Enemy e in GameManager.instance.Enemies)
        {
            e.SetVisor(visorOn);
        }
    }
}
