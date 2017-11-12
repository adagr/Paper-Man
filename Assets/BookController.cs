using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookController: MonoBehaviour {

    public GameObject player;
    public GameObject spawnEffect;

    private Vector3 initialPlayerScale;
    private Vector3 initialPlayerPosition;

    private bool playerHasSpawned = false;

	// Use this for initialization
	void Start () {
        initialPlayerPosition = player.transform.position;
        initialPlayerScale = player.transform.localScale;

        player.transform.localScale = new Vector3(0, 0, 0);

        Vector3 playerPosition = transform.position;
        playerPosition.y += .5f;
        player.transform.position = initialPlayerPosition;
    }
	
	// Update is called once per frame
	void Update () {
        if (!playerHasSpawned)
        {
            player.transform.position = initialPlayerPosition;
        }
	}

    public void BookIsOpened()
    {
        Vector3 effectPosition = transform.position;
        effectPosition.y += .5f;
        Instantiate(spawnEffect, effectPosition, transform.rotation);

        Invoke("SpawnPlayer", 1);
    }

    private void SpawnPlayer()
    {
        playerHasSpawned = true;
        player.transform.localScale = initialPlayerScale;
    }
}
