using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float speed = 1;
    public float rotationSpeed = 1;
    private CharacterController cc;
    private Quaternion rotationTarget;
    private int currentRotation = 0;
    private CollisionScript cs;
	// Use this for initialization
	void Start () {
        cc = GetComponent<CharacterController>();
        cs = FindObjectOfType<CollisionScript>();
        rotationTarget = new Quaternion();
	}
	
	// Update is called once per frame
	void Update ()
    {

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
        //cc.Move(new Vector3(Mathf.Clamp(x,-1,1), 0, Mathf.Clamp(z, -1, 1)) * speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x + Mathf.Clamp(x, -1, 1) * Time.deltaTime * speed, 0.5f, transform.position.z + Mathf.Clamp(z, -1, 1) * Time.deltaTime * speed);

        if (cs.canRotate)
        {
            if (Input.GetKeyDown("e"))
                currentRotation = (currentRotation + 90) % 360;
            else if (Input.GetKeyDown("q"))
                currentRotation = (currentRotation - 90) % 360;
            rotationTarget.eulerAngles = new Vector3(0, currentRotation, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget, Time.deltaTime * rotationSpeed);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.transform.name);
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exit");
    }
}
