using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnOnFall : MonoBehaviour {
    public Transform spawn;

    public void Respawn() {
        transform.position = spawn.position;
        if (GetComponent<Rigidbody>() != null) GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
