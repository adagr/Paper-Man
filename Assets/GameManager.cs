using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public bool isDead = false;
    public AudioClip backgroundSound, frogSound;
    private AudioSource source;
    private Movement paperMan;
    private bool playFrog = false;
	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        source.clip = backgroundSound;
        source.loop = true;
        source.Play();
        paperMan = FindObjectOfType<Movement>();
    }
	
	// Update is called once per frame
	void Update () {
		if (isDead)
        {
            if (Input.GetKeyDown("r"))
                SceneManager.LoadScene(1);
        }
        if (paperMan.isFrog && !playFrog)
        {
            source.Stop();
            source.clip = frogSound;
            source.loop = true;
            source.Play();
            playFrog = true;
        } else if (!paperMan.isFrog && playFrog)
        {
            source.Stop();
            source.clip = backgroundSound;
            source.loop = true;
            source.Play();
            playFrog = false;
        }
        if (Input.GetKeyDown("escape"))
            SceneManager.LoadScene(0);
	}
    
}
