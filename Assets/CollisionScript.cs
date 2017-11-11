using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionScript : MonoBehaviour {

    public bool canRotate = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "PaperPowerup" && other.tag != "Wind")
            canRotate = false;
    }

    private void OnTriggerExit(Collider other)
    {
        canRotate = true;
    }
}
