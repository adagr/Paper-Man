using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float speed = 1;
    public float rotationSpeed = 1;
    private CharacterController cc;
    private Quaternion rotationTarget;
    private int currentRotation = 0, cameraRotation = 0;
    private CollisionScript cs;
    private Rigidbody rb;
    private Animator animator;
    private int rx = 1, rz = 1, rxa = 1, rza = 1;
	// Use this for initialization
	void Start () {
        cc = GetComponent<CharacterController>();
        cs = FindObjectOfType<CollisionScript>();
        rb = GetComponent<Rigidbody>();
        rotationTarget = new Quaternion();
        animator = GetComponentInChildren<Animator>();
        animator.speed = 5; 
	}

    private void Update()
    {
        if (Input.GetKeyDown("z"))
            cameraRotation = (cameraRotation + 90) % 360;
        else if (Input.GetKeyDown("c"))
            cameraRotation = (cameraRotation - 90) % 360;
        if ((cameraRotation + 360) % 360 == 0)
        {
            rx = 1;
            rz = 1;
            rxa = 1;
            rza = 1;
        }
        else if ((cameraRotation + 360) % 360 == 90)
        {
            rx = 1;
            rz = -1;
            rxa = -1;
            rza = 1;
        }
        else if ((cameraRotation + 360) % 360 == 180)
        {
            rx = -1;
            rz = -1;
            rxa = -1;
            rza = -1;
        }
        else if ((cameraRotation + 360) % 360 == 270)
        {
            rx = -1;
            rz = 1;
            rxa = 1;
            rza = -1;
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {

        

        float x = 0;
        float z = 0;
        if (Input.GetKey("s"))
        {
            x += -1 * rx;
            z += -1 * rz;
        }
        else if (Input.GetKey("w"))
        {
            x += 1 * rx;
            z += 1 * rz;
        }
        if (Input.GetKey("a"))
        {
            x += -1 * rxa;
            z += 1 * rza;
        }
        else if (Input.GetKey("d"))
        {
            x += 1 * rxa;
            z += -1 * rza;
        }

        rb.velocity = new Vector3(x, 0, z) * speed;

        if (x != 0 || z != 0)
            animator.SetBool("IsWalking", true);
        else
            animator.SetBool("IsWalking", false);

        //rb.MovePosition(new Vector3(transform.position.x + Mathf.Clamp(x, -1, 1) * Time.deltaTime * speed, 0.5f, transform.position.z + Mathf.Clamp(z, -1, 1) * Time.deltaTime * speed));

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
