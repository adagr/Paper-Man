using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    public float speed = 1;
    public float rotationSpeed = 1;
    private CharacterController cc;
    private Quaternion rotationTarget;
    public int currentRotation = 0, cameraRotation = 0;
    private CollisionScript cs;
    private Rigidbody rb;
    private Animator[] animators;
    private int rx = 1, rz = 1, rxa = 1, rza = 1;
    private Clone[] clones;
    private int activeClones = 1;
    private CapsuleCollider col;
    private bool invincible = false;
    private float invincibleTimer;
    public float invincibleTime = 2;
    private float x = 0, z = 0;
    private Frog frog;
    public AudioClip deathSound;
    private GameManager gm;
    public bool isFrog = false, canBeFrog = false;
    public ParticleSystem blood;
    // Use this for initialization
    void Start () {
        cc = GetComponent<CharacterController>();
        cs = FindObjectOfType<CollisionScript>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();
        rotationTarget = new Quaternion();
        animators = GetComponentsInChildren<Animator>();
        foreach (Animator animator in animators)
            animator.speed = 5;
        clones = FindObjectsOfType<Clone>();
        foreach (Clone clone in clones)
            clone.gameObject.SetActive(false);
        invincibleTimer = invincibleTime;
        frog = GetComponentInChildren<Frog>();
        frog.gameObject.SetActive(false);
        gm = FindObjectOfType<GameManager>();

    }

    private void Update()
    {
        if (gm.isDead)
            return;
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
        x = 0;
        z = 0;
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

        if (cs.canRotate)
        {
            if (Input.GetKeyDown("e"))
                currentRotation = (currentRotation + 90) % 360;
            else if (Input.GetKeyDown("q"))
                currentRotation = (currentRotation - 90) % 360;
            rotationTarget.eulerAngles = new Vector3(0, currentRotation, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotationTarget, Time.deltaTime * rotationSpeed);
        }

        var walk = false;
        if (x != 0 || z != 0)
            walk = true;
        foreach (Animator animator in animators)
            if (animator.gameObject.activeSelf)
                animator.SetBool("IsWalking", walk);

        if (canBeFrog && Input.GetKeyDown("f"))
        {
            if (isFrog)
                turnIntoMan();
            else
                turnIntoFrog();
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (gm.isDead)
            return;
        if (invincible)
        {
            invincibleTimer -= Time.fixedDeltaTime;
            if (invincibleTimer < 0)
            {
                invincible = false;
                invincibleTimer = invincibleTime;
            }
        }

        rb.velocity = new Vector3(x, 0, z) * speed;
    }

    public void activateClone(GameObject powerup)
    {
        foreach (Clone clone in clones)
        {
            if (activeClones+1 <= clones.Length && clone.name.CompareTo("PaperMan" + (activeClones+1)) == 0)
            {
                activeClones++;
                if (!isFrog)
                    clone.gameObject.SetActive(true);
                col.radius += 0.02f;
                AudioSource.PlayClipAtPoint(powerup.GetComponentInParent<Damage>().clip, powerup.transform.position);
                break;
            }
        }
    }
    public void deactivateClone(GameObject enemy)
    {
        if (invincible)
            return;
        foreach (Clone clone in clones)
        {
            if (activeClones > 1 && clone.name.CompareTo("PaperMan" + activeClones) == 0)
            {
                clone.gameObject.SetActive(false);
                col.radius -= 0.02f;
                activeClones--;
                invincible = true;
                AudioSource.PlayClipAtPoint(enemy.GetComponentInParent<Damage>().clip, enemy.transform.position);
                break;
            } else if (activeClones == 1)
            {
                if (!gm.isDead)
                {
                    AudioSource.PlayClipAtPoint(deathSound, transform.position);
                    Destroy(this.gameObject, 0);
                    Instantiate(blood, transform.position, new Quaternion());
                    gm.isDead = true;
                }
            }
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PaperPowerup")
        {
            activateClone(collision.gameObject);
            Destroy(collision.gameObject);
        } else if (collision.gameObject.tag == "Enemy")
        {
            deactivateClone(collision.gameObject);
        } else if (collision.gameObject.tag == "FrogPowerup")
        {
            canBeFrog = true;
            isFrog = true;
            turnIntoFrog();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Wind")
        {
            WindScript wind = other.gameObject.GetComponent<WindScript>();
            Vector3 dir = other.gameObject.GetComponent<WindScript>().direction;
            float force = 0;
            if (dir.x != 0)
                if (Mathf.Abs(currentRotation) == 90 || Mathf.Abs(currentRotation) == 270 || isFrog)
                    force = wind.windPower / Mathf.Clamp((Mathf.Abs(transform.position.x - wind.point.transform.position.x)),0.5f,10);
                else
                    force = 10 / Mathf.Clamp((Mathf.Abs(transform.position.x - wind.point.transform.position.x)), 0.5f, 10);
            if (dir.z != 0)
                if (Mathf.Abs(currentRotation) == 180 || currentRotation == 0 || isFrog)
                    force = wind.windPower / Mathf.Clamp((Mathf.Abs(transform.position.z - wind.point.transform.position.z)), 0.5f, 10);
                else
                    force = 10 / Mathf.Clamp((Mathf.Abs(transform.position.z - wind.point.transform.position.z)), 0.5f, 10);
            rb.AddForce(dir * force);
        }
    }

    private void turnIntoFrog()
    {
        foreach(Animator anim in animators)
        {
            anim.gameObject.SetActive(false);
        }
        frog.gameObject.SetActive(true);
        isFrog = true;
    }
    private void turnIntoMan()
    {
        int lives = activeClones;
        foreach (Animator anim in animators)
        {
            if (lives > 0)
                anim.gameObject.SetActive(true);
            else
                break;
            lives--;
        }
        frog.gameObject.SetActive(false);
        isFrog = false;
    }
}
