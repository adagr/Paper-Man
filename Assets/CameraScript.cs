﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public Transform point;
    public float rotationSpeed = 1;
    private Movement player;
    private Vector3 distance;
    private float currentRotation;
    private Quaternion rotationTarget;
    // Use this for initialization
    void Start () {
        player = FindObjectOfType<Movement>();
        distance = transform.position - player.transform.position;
        rotationTarget = new Quaternion();
        currentRotation = transform.rotation.eulerAngles.y;
    }
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + distance, Time.deltaTime*2);
        /*if (Input.GetKeyDown("z"))
            transform.RotateAround(point.position, new Vector3(0, 1,0), 90);
        if (Input.GetKeyDown("c"))
            transform.RotateAround(point.position, new Vector3(0, 1, 0), -90);
        */
        if (Input.GetKeyDown("z"))
            currentRotation = (currentRotation + 90) % 360;
        else if (Input.GetKeyDown("c"))
            currentRotation = (currentRotation - 90) % 360;
        rotationTarget.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, currentRotation, transform.rotation.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget, Time.deltaTime * rotationSpeed);
    }
}
