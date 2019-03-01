using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sworder : MonoBehaviour
{
    public float maxSpeed;
    public float accel;
    public float attackDist;
    public float range;
    Rigidbody rigid;
    GameObject player;
    Vector3 velVec;
    bool attacking = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {

    }
    IEnumerator SlowDown()
    {
        float timeSlowing = 0.0f;
        while (Mathf.Abs(velVec.x) > 0.05f && timeSlowing < 1.0f)
        {
            timeSlowing += Time.deltaTime;
            if (velVec.x > 0.1f)
                velVec.x -= accel * Time.deltaTime;
            else if (velVec.x < -0.1f)
                velVec.x += accel * Time.deltaTime;
            else
                velVec.x -= velVec.x;

            rigid.velocity = velVec;
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }

    IEnumerator Attack(bool left)
    {
        attacking = true;
        StartCoroutine(SlowDown());
        yield return new WaitForSeconds(0.25f);
        //Animation Play
      /*  gameObject.transform.GetChild(0).GetComponent<Animator>().SetInteger("IsSlashing", 1);
        if (left)
            transform.GetChild(0).gameObject.SetActive(true);
        else
            transform.GetChild(1).gameObject.SetActive(true);*/

        yield return new WaitForSeconds(1.0f);

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        attacking = false;
    }

    void Update()
    {
        if (!GameManager._paused && GameObject.FindGameObjectWithTag("Player") != null)
        {
            velVec = rigid.velocity;
            if (!attacking && Mathf.Abs(transform.position.x - player.transform.position.x) < range)
            {
                if (Mathf.Abs(transform.position.y - player.transform.position.y) < 2.0f)
                {
                    //Hunt for player
                    if (transform.position.x < player.transform.position.x)
                    {
                        if (velVec.x < maxSpeed)
                            velVec.x += accel * Time.deltaTime;
                    }
                    if (transform.position.x > player.transform.position.x)
                    {
                        if (velVec.x > -maxSpeed)
                            velVec.x -= accel * Time.deltaTime;
                    }

                    if (Mathf.Abs(transform.position.x - player.transform.position.x) < attackDist)
                    {
                        if (transform.position.x > player.transform.position.x)
                            StartCoroutine(Attack(true));
                        else
                            StartCoroutine(Attack(false));
                    }
                }
                else
                {
                    if (velVec.x > 0.1f)
                        velVec.x -= accel * Time.deltaTime;
                    else if (velVec.x < -0.1f)
                        velVec.x += accel * Time.deltaTime;
                    else
                        velVec.x -= velVec.x;
                }
            }
            rigid.velocity = velVec; 
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