using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PointRemovingObject : MonoBehaviour
{
    public int pointAmount = 5;

    private void OnCollisionEnter(Collision collision) {
        if (collision.transform.tag == "Player") {
            GameManager.ChangeScore(collision.transform.GetComponent<FriendController>().playerNum, -pointAmount);
        }
        StartCoroutine(BlinkOut());
    }

    IEnumerator BlinkOut() {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
