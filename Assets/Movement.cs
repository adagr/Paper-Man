using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float speed = 1;
    public float rotationSpeed = 1;
    private CharacterController cc;
	// Use this for initialization
	void Start () {
        cc = GetComponent<CharacterController>();   
	}
	
	// Update is called once per frame
	void Update () {

        float x = 0;
        float y = 0;
        float z = 0;
        if (Input.GetKey("s"))
        {
            x += -1;
            z += -1;
        }
        else if (Input.GetKey("w"))
        {
            x += 1;
            z += 1;
        }
        if (Input.GetKey("a"))
        {
            x += -1;
            z += 1;
        }
        else if (Input.GetKey("d"))
        {
            x += 1;
            z += -1;
        }
        cc.Move(new Vector3(Mathf.Clamp(x,-1,1), 0, Mathf.Clamp(z, -1, 1)) * speed * Time.deltaTime);

        if (Input.GetKey("q"))
            y = -1;
        else if (Input.GetKey("e"))
            y = 1;
        transform.Rotate(0, y * rotationSpeed, 0);
    }
    
}
