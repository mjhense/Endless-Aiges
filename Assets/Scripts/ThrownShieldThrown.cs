using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownShieldThrown : MonoBehaviour
{
    Rigidbody rigid;

    public float rollingFriction;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.maxAngularVelocity = 40.0f;
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        rigid.velocity *= 1.0f - rollingFriction * Time.deltaTime;
        //rigid.angularVelocity -= rollingFriction * Vector3.forward * Mathf.Sign(rigid.angularVelocity.z) * Time.deltaTime;
    }
}