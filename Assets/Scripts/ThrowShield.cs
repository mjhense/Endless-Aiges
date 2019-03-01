using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowShield : MonoBehaviour
{

    public GameObject shieldPrefab;
    public static bool hasShield = false;
    public GameObject shield;
    GameObject indicator;
    bool aiming = false;

    private void Awake()
    {
        indicator = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetChild(2).gameObject;
        indicator.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    IEnumerator HuckIt()
    {
        float angle = transform.GetChild(0).transform.eulerAngles.z * Mathf.Deg2Rad;

        if (angle * Mathf.Rad2Deg > 90.0f && angle * Mathf.Rad2Deg < 270.0f)
            AnimationStateController._state = 9;
        else
            AnimationStateController._state = 8;
        yield return new WaitForSeconds(0.1f);
        indicator.SetActive(false);
        shield = Instantiate(shieldPrefab,
            new Vector3(transform.position.x * Mathf.Cos(angle), transform.position.y * Mathf.Sin(angle)),
            Quaternion.Euler(0, 0, 0));
        shield.transform.position = transform.position;
        // Kind of experimental spinning
        shield.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, -20 * ((AnimationStateController._state % 2 == 0) ? 1 : -1));
        Physics.IgnoreCollision(GetComponent<CapsuleCollider>(), shield.GetComponent<SphereCollider>());
        shield.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0.0f) * 25.0f;
        hasShield = false;
        aiming = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!InputManager._block)
        {
            if (InputManager._aimPressed && hasShield)
            {
                aiming = true;
                indicator.SetActive(true);
            }

            if (InputManager._release && aiming)
            {
                if (hasShield)
                {
                    StartCoroutine(HuckIt());
                }
            }
        }
        else
            indicator.SetActive(false);

        if (!hasShield && shield != null)
        {
            if (Vector3.Distance(shield.transform.position, transform.position) < 2)
            {
                if (InputManager._pickup)
                {
                    hasShield = true;
                    Destroy(shield);
                }
            }
        }
    }
}
