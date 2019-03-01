using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{

    public GameObject arrowPrefab;
    public float range;

    float shootVel = 30.0f;
    float gravity = -30.0f;
    float timeOfNextShot;

    // Start is called before the first frame update
    void Start()
    {
        timeOfNextShot = Time.time + 3.0f;   
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager._paused && GameObject.FindGameObjectWithTag("Player") != null && Mathf.Abs(GameObject.FindGameObjectWithTag("Player").transform.position.x - transform.position.x) < range)
        {
            Vector3 delta = GameObject.FindGameObjectWithTag("Player").transform.position - transform.position;
            float shootVel2 = shootVel * shootVel;
            float range = shootVel2 * shootVel2 + 2 * delta.y * gravity * shootVel2 - gravity * gravity * delta.x * delta.x;

            // If an arrow can reach you
            if (range > 0)
            {
                float vx1 = -gravity * delta.x / Mathf.Sqrt(2 * (delta.y * gravity + shootVel2 + Mathf.Sqrt(range)));
                float vx2 = -gravity * delta.x / Mathf.Sqrt(2 * (delta.y * gravity + shootVel2 - Mathf.Sqrt(range)));

                float vy1 = Mathf.Sqrt(shootVel2 - vx1 * vx1);
                float vy2 = Mathf.Sqrt(shootVel2 - vx2 * vx2);

                float vx = vx1;
                float vy = vy1;
                if (Mathf.Abs(vx2) > Mathf.Abs(vx1))
                {
                    vx = vx2;
                    vy = vy2;
                }

                if (Mathf.Abs(vy * delta.x / vx + gravity * delta.x * delta.x / vx / vx / 2 - delta.y) > Mathf.Abs(-vy * delta.x / vx + gravity * delta.x * delta.x / vx / vx / 2 - delta.y))
                {
                    vy *= -1;
                }

                if (float.IsNaN(vx) || float.IsNaN(vy))
                {
                    Debug.Log("Arrow NaN. You should never see this message.");
                }
                else
                {
                    if (Time.time >= timeOfNextShot)
                    {
                        gameObject.transform.GetChild(0).GetComponent<Animator>().SetInteger("fire", 1);
                        GameObject arrow = Instantiate(arrowPrefab);
                        arrow.transform.position = transform.position;
                        arrow.GetComponent<Rigidbody>().velocity = new Vector3(vx * Random.Range(0.9f, 1.1f), vy + Random.Range(0.9f, 1.1f), 0.0f);
                        timeOfNextShot = Time.time + 3.0f + Random.Range(-0.5f, 0.5f);
                    }
                }
            }
            // If you are out of range of the archer
            else
            {

            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If they are hit by a reflected arrow
        if (collision.gameObject.tag == "ReflectedArrow")
        {
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}