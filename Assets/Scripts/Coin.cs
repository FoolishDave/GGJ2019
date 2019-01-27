using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private int dir;
    public float rotSpeed;

    void Awake() {
        dir = Random.Range(0f,1f) > .5f ? -1 : 1;
        rotSpeed = Random.Range(90f, 120f);
    }

    void Update() {
        transform.Rotate(0,rotSpeed*dir*Time.deltaTime,0);
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            GameManager.ChangeScore(other.GetComponent<FriendController>().playerNum,1);
            Destroy(gameObject);
        }
    }
}
