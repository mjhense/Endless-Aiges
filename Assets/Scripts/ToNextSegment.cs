using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToNextSegment : MonoBehaviour
{
    public GameObject[] needToBeKilledFirst;
    public Transform newSpawnPoint;
    GameObject player;
    GameObject cam;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void Update()
    {
        if (CanMoveOn())
        {
            player.transform.position = newSpawnPoint.position;
            cam.transform.position = new Vector3(newSpawnPoint.position.x, newSpawnPoint.position.y, -10.0f);

            if (GameObject.FindGameObjectWithTag("ThrownShield") != null)
                GameObject.FindGameObjectWithTag("ThrownShield").transform.position = 
                    new Vector3(newSpawnPoint.position.x + 1.0f, newSpawnPoint.position.y, 0.0f);

            Destroy(gameObject);
        }
    }

    private bool CanMoveOn()
    {
        bool cmo = true;
        for (int i = 0; i < needToBeKilledFirst.Length; i++)
            if (needToBeKilledFirst[i] != null)
                cmo = false;
        return cmo;
    }
}