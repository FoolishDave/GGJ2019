using Rewired;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendController : MonoBehaviour
{
    public int playerId = 0;
    public float speed = .75f;
    public float maxSpeed = 1.5f;

    private Rigidbody rigid;
    private Player player;
    private Vector3 movementVector;
    private bool jump;
    private bool interact;

    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ProcessInput();
        HandleInput();
    }

    private void ProcessInput()
    {
        //butts
        movementVector.x = player.GetAxis("Horizontal");
        movementVector.z = player.GetAxis("Vertical");
        jump = player.GetButton("Jump");
        interact = player.GetButton("Interact");
    }

    private void HandleInput()
    {
        Vector3 lookPos = transform.position + movementVector;
        transform.LookAt(lookPos);
        float moveSpeed = movementVector.magnitude;
        if (moveSpeed < 0f) moveSpeed *= -1f;
        if (moveSpeed > 1f) moveSpeed = 1f;

        if (moveSpeed > 0.1f)
        {
            Debug.Log("Player: " + playerId + ": " + lookPos);
        }

        if (rigid.velocity.sqrMagnitude < maxSpeed * maxSpeed)
            rigid.AddForce(transform.forward.normalized * speed);

        jump = false;
        interact = false;
    }
}
