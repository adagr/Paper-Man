using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {

    public float jumpPower = 100, gravity = 100;
    private Rigidbody rb;
    private float height, jumptimer;
    public float jumptime = 1;
    private bool jumping = false;
    public Frog frog;
    public bool canJump = false;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        height = transform.position.y;
        jumptimer = jumptime;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        LayerMask mask = 1 << 8;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10, mask))
        {
            height = hit.collider.gameObject.transform.position.y + hit.collider.bounds.extents.y + 0.4f;
        }
        jumptimer -= Time.deltaTime;
        if (jumping)
        {
            if (jumptimer < 0)
            {
                jumping = false;
            }
            rb.AddForce(new Vector3(0, 1, 0) * jumpPower * jumptimer);
        } else
        {
            if (transform.position.y - height > 0.1f)
            {
                rb.AddForce(new Vector3(0, -1, 0) * gravity);
            } else
                transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, height, transform.position.z), Time.fixedDeltaTime);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown("space") && jumptimer < -jumptime && frog && frog.gameObject.activeSelf)
        {
            jumping = true;
            jumptimer = jumptime;
        }
        if (canJump && Input.GetKeyDown("space"))
            jumping = true;
    }
}
