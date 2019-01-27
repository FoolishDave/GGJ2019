using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningParticle : MonoBehaviour
{
    ParticleSystem ps;
    Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        rigid = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(rigid.velocity.magnitude);
    }
}
