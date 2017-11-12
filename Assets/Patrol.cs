using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{

    public Transform pointA, pointB;
    private Vector3 target;
    public float speed = 1;
    private Vector3 a, b;
    // Use this for initialization
    void Start()
    {
        a = pointA.position;
        b = pointB.position;
        target = a;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distance = target - transform.position;
        transform.position += distance.normalized * Time.deltaTime * speed;
        Quaternion q = new Quaternion();
        q.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, (target==a)?0:180, -89);
        //transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * 5);
        //transform.LookAt(target);
        if (distance.magnitude < 0.1)
        {
            if (target == a)
                target = b;
            else
                target = a;
        }
        Debug.Log(transform.rotation.eulerAngles.z+ " " + transform.rotation.z);
    }
}
