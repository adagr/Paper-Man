using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public Movement player;
    private Quaternion fixedRotation;
    private Vector3 distance;
	// Use this for initialization
	void Start () {
        fixedRotation = transform.rotation;
        player = FindObjectOfType<Movement>();
        distance = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + distance, Time.deltaTime);
	}
}
