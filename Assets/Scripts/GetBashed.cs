using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBashed : MonoBehaviour
{
    public float bashSpeed;
    public float thrownBashSpeed;
    public float friction;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Shield")
        {
            float zAngle = other.gameObject.transform.parent.eulerAngles.z;
            float xComp = Mathf.Abs(Mathf.Cos(zAngle)) * bashSpeed;
            float yComp = Mathf.Abs(Mathf.Sin(zAngle)) * bashSpeed;
            if (zAngle < 30.0f && zAngle > 120.0f)
            {
                xComp = bashSpeed;
                yComp = bashSpeed / 3.0f;
            }
            else
            {
                xComp = bashSpeed / 3.0f;
                yComp = 2.0f * bashSpeed / 3.0f;
            }

            if (zAngle > 90.0f && zAngle < 270.0f)
                xComp = -xComp;

            GetComponent<Rigidbody>().velocity = new Vector3(xComp, yComp);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ThrownShield" && collision.gameObject.GetComponent<Rigidbody>().velocity.x != 0.0f && 
            collision.gameObject.GetComponent<Rigidbody>().velocity.y != 0.0f)
        {
            float zAngle = Mathf.Atan2(transform.position.y - collision.transform.position.y, transform.position.x - collision.transform.position.x);
            float xComp = Mathf.Abs(Mathf.Cos(zAngle)) * thrownBashSpeed;
            if (Mathf.Abs(zAngle) > Mathf.PI / 2.0f)
                xComp = -xComp;
            float yComp = Mathf.Abs(Mathf.Sin(zAngle)) * thrownBashSpeed;
            if (zAngle > 0.0f)
                yComp = -yComp;

            GetComponent<Rigidbody>().velocity = new Vector3(xComp, yComp);
        }
        /*
        if (collision.gameObject.tag == "ThrownShield" && gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
        */
    }

    private void OnCollisionStay(Collision collision)
    {
        Vector3 newVel = GetComponent<Rigidbody>().velocity;

        if (newVel.x > 0.1f)
            newVel.x -= friction * Time.deltaTime;
        else if (newVel.x < -0.1f)
            newVel.x += friction * Time.deltaTime;
        else
            newVel.x = 0.0f;

        GetComponent<Rigidbody>().velocity = newVel;
    }
}