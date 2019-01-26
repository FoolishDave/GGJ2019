using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public bool leftGoal;
    public Soccer soccer;

    void OnTriggerEnter(Collider other) {
        if (other.tag == "SoccerBall") {
            soccer.Goal(leftGoal);
        }
    }
}
