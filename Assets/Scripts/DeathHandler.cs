using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{

    void Start()
    {

    }


    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(other.gameObject);
            GameManager._playerDeath = true;
        }
        else if (other.gameObject.tag == "ThrownShield")
            GameManager._shieldDeath = true;
        else if (other.gameObject.tag == "Enemy")
            Destroy(other.gameObject);
    }
}