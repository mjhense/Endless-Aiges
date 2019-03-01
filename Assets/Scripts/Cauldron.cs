using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cauldron : MonoBehaviour
{

    public float timer = 1.0f;
    public float elapsed = 1.0f;
    public Vector3 origin = new Vector3(-90, 90, 0);
    public float rotateTimer = 1.0f;
    public Vector3 goalVector = new Vector3(-180, 90, 0);
    private float rotateSpeed = 0.0f;
    private bool rotateDown = true;
    private float degree180 = 18.0f;
    private float degree0 = 270.0f;
    private ParticleSystem oil;
    public List<ParticleCollisionEvent> collisionEvents;

    // Start is called before the first frame update
    void Start()
    {
        rotateSpeed = Mathf.Abs(origin.x - goalVector.x) / rotateTimer;
        rotateSpeed *= -1;
        oil = gameObject.transform.GetChild(2).GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
        oil.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
        /*
        elapsed += Time.deltaTime;
        if (elapsed > timer)
        {
            elapsed = 0.0f;
            //spawn shit
            rotateDown = true;
        }
        */

        if (rotateDown)
        {
            gameObject.transform.GetChild(0).transform.rotation *= Quaternion.Euler(new Vector3(rotateSpeed, 0, 0) * Time.deltaTime);
        }
        else
        {
            gameObject.transform.GetChild(0).transform.rotation *= Quaternion.Euler(new Vector3(-rotateSpeed, 0, 0) * Time.deltaTime);
        }
        
        if (rotateDown && gameObject.transform.GetChild(0).transform.eulerAngles.x <= degree180)
        {
            rotateDown = false;
            //spill shit
            oil.Play();
        }
        if (!rotateDown && gameObject.transform.GetChild(0).transform.rotation.x >= (0))
        {
            rotateDown = true;
        }
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = oil.GetCollisionEvents(other, collisionEvents);

        Rigidbody rb = other.GetComponent<Rigidbody>();
        int i = 0;
        Debug.Log("yes");
        while (i < numCollisionEvents)
        {
            if (rb)
            {
                Vector3 pos = collisionEvents[i].intersection;
                Vector3 force = collisionEvents[i].velocity * 10;
                rb.AddForce(force);
            }
            i++;
        }
    }
}
