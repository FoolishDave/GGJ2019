using Rewired;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FriendController : MonoBehaviour
{
    public int playerId = 0;
    public float speed = .75f;
    public float throwAngle = 45f;
    public float throwForce = 5f;

    public float pickupOffset = 1.1f;
    public Vector3 pickupBox = new Vector3(.3f, .6f, .3f);
    public GameObject pickupPoint;

    private GameObject holding;
    private Rigidbody rigid;
    private Player player;
    private Vector3 movementVector;
    private bool jump;
    private bool interact;

    void Start()
    {
        player = ReInput.players.GetPlayer(playerId);
        player.isPlaying = true;
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
        interact = player.GetButtonDown("Interact");
    }

    private void HandleInput()
    {
        Vector3 lookPos = transform.position + movementVector;
        transform.LookAt(lookPos);
        float moveSpeed = movementVector.magnitude;
        if (moveSpeed < 0f) moveSpeed *= -1f;
        if (moveSpeed > 1f) moveSpeed = 1f;

        Vector3 movePos = transform.position + transform.forward.normalized * moveSpeed * speed;
        movePos.y = transform.position.y;
        rigid.MovePosition(movePos);

        if (interact && holding == null)
        {
            Pickup();
        } else if (interact && holding != null)
        {
            Throw();
        }

        jump = false;
        interact = false;
    }

    private void Pickup()
    {

        IEnumerable<Collider> colliders = Physics.OverlapBox(transform.position + transform.forward * pickupOffset, pickupBox).Where(x => x.transform != transform && x.tag == "Pickup");
        if (!colliders.Any()) return;
        Collider pickupTarget = colliders.Aggregate((i1, i2) => Vector3.Distance(i1.transform.position, transform.position) < Vector3.Distance(i2.transform.position, transform.position) ? i1 : i2);
        if (pickupTarget != null)
        {
            Debug.Log("Picking up " + pickupTarget.name);
            pickupTarget.transform.parent = pickupPoint.transform;
            pickupTarget.transform.localPosition = Vector3.zero;
            holding = pickupTarget.gameObject;
            holding.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void Throw()
    {
        Debug.Log("Throwing");
        Rigidbody heldRigid = holding.GetComponent<Rigidbody>();
        heldRigid.isKinematic = false;
        holding.transform.parent = transform.parent;
        holding = null;
        Vector3 throwVector = transform.forward.normalized;
        throwVector = Quaternion.AngleAxis(throwAngle, transform.right) * throwVector * throwForce;
        heldRigid.AddForce(throwVector);
    }
}
