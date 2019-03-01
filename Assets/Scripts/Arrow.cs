using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (GetComponent<Rigidbody>() != null)
        {
            float xAngle = 180.0f * Mathf.Atan2(GetComponent<Rigidbody>().velocity.y, GetComponent<Rigidbody>().velocity.x) / Mathf.PI;
            transform.eulerAngles = new Vector3(xAngle, -90.0f, 0.0f);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag != "Shield")
        {
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<SphereCollider>());

            if (collision.collider.gameObject.tag != "Ground")
                transform.parent = collision.collider.gameObject.transform;

            if (collision.collider.gameObject == GameObject.FindGameObjectWithTag("Player"))
                GameManager.hits++;
        }
        else
        {
            Vector2 i = new Vector2(GetComponent<Rigidbody>().velocity.x, GetComponent<Rigidbody>().velocity.y);
            Vector2 u = new Vector2(Mathf.Cos(Mathf.Deg2Rad * collision.collider.transform.eulerAngles.z), Mathf.Sin(Mathf.Deg2Rad * collision.collider.transform.eulerAngles.z));
            Vector2 f = new Vector2(-i.x - 2 * u.y * (-i.x * u.y + i.y * u.x), -2 * i.x * u.x * u.y + i.y * u.x * u.x - i.y * u.y * u.y);
            f *= 2.0f;

            GetComponent<Rigidbody>().velocity = new Vector3(u.x, u.y, 0.0f);
            GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * 30.0f;

            tag = "ReflectedArrow";
            gameObject.layer = 11;
        }
    }
}