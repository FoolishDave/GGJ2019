using DG.Tweening;
using Rewired;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FriendController : MonoBehaviour
{
    // Probably should be a getter method but can't be bothered.
    public int playerId = 0;
    // Things to be tweaked ewe
    public float speed = .75f;
    public float throwAngle = 45f;
    public float throwForce = 5f;
    public float pickupOffset = 1.1f;
    public float rotateSpeed = 200f;
    public float knockoverImpulse = .3f;
    public float knockdownTime = .2f;
    public float knockdownCooldown = 2f;
    public Vector3 pickupBox = new Vector3(.3f, .6f, .3f);
    public GameObject pickupPoint;

    // Things to keep track of owo
    private GameObject holding;
    private Rigidbody rigid;
    private Player player;
    private Vector3 movementVector;
    // Input variables uwu
    private bool jump;
    private bool interact;
    private bool leave;
    private bool throwHeld;
    private bool knockedDown;
    private float knockdownTimer;

    void Start() {
        player = ReInput.players.GetPlayer(playerId);
        rigid = GetComponent<Rigidbody>();
    }

    void Update() {
        ProcessInput();
        HandleInput();

        if (knockdownTimer > 0f) {
            knockdownTimer -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (!knockedDown && knockdownTimer <= 0f && collision.impulse.sqrMagnitude > knockoverImpulse) {
            Debug.Log("Knocked down.");
            rigid.constraints = RigidbodyConstraints.None;
            knockedDown = true;
            knockdownTimer = knockdownTime * collision.impulse.sqrMagnitude;
        }
    }

    private void ProcessInput() {
        //butts
        movementVector.x = player.GetAxis("Horizontal");
        movementVector.z = player.GetAxis("Vertical");
        jump = player.GetButton("Jump");
        interact = player.GetButtonDown("Interact");
        leave = player.GetButtonDown("Leave");
        throwHeld = player.GetButtonDown("Throw");
    }

    private void HandleInput() {
        if (leave) {
            PlayerManager.Instance.DespawnPlayer(player);
            return;
        }

        if (knockedDown) {
            if (jump && knockdownTimer < 0) {
                Standup();
            } else {
                return;
            }
        }

        float rotateAmount = Vector3.Cross(movementVector.normalized, transform.forward).y;
        Vector3 angular = rigid.angularVelocity;
        angular.y = -rotateAmount * rotateSpeed;
        rigid.angularVelocity = angular;
        float moveSpeed = movementVector.magnitude;
        if (moveSpeed < 0f) moveSpeed *= -1f;
        if (moveSpeed > 1f) moveSpeed = 1f;

        Vector3 movePos = transform.position + transform.forward.normalized * moveSpeed * speed;
        movePos.y = transform.position.y;
        rigid.MovePosition(movePos);

        if (interact && holding == null) {
            Pickup();
        } else if (interact && holding != null) {
            UseItem();
        }

        if (throwHeld && holding != null) {
            Throw();
        }
    }

    private void Pickup() {

        IEnumerable<Collider> colliders = Physics.OverlapBox(transform.position + transform.forward * pickupOffset, pickupBox).Where(x => x.transform != transform && x.tag == "Pickup");
        if (!colliders.Any()) return;
        Collider pickupTarget = colliders.Aggregate((i1, i2) => Vector3.Distance(i1.transform.position, transform.position) < Vector3.Distance(i2.transform.position, transform.position) ? i1 : i2);
        if (pickupTarget != null) {
            pickupTarget.transform.parent = pickupPoint.transform;
            pickupTarget.transform.localPosition = Vector3.zero;
            holding = pickupTarget.gameObject;
            holding.GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    private void UseItem() {

    }

    private void Throw() {
        Rigidbody heldRigid = holding.GetComponent<Rigidbody>();
        heldRigid.isKinematic = false;
        holding.transform.parent = transform.parent;
        holding = null;
        Vector3 throwVector = transform.forward.normalized;
        throwVector = Quaternion.AngleAxis(throwAngle, transform.right) * throwVector * throwForce;
        heldRigid.AddForce(throwVector);
    }

    public void Respawn() {
        transform.position = new Vector3(Random.Range(-2.5f, 2.5f), 4, Random.Range(-1f, 1f));
        knockedDown = true;
        knockdownTimer = 1f;
        transform.eulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
    }

    private void Standup() {
        rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        knockedDown = false;
        knockdownTimer = knockdownCooldown;
        transform.DOMoveY(transform.position.y + .2f, .2f);
        transform.DORotate(Vector3.zero, .3f);
    }
}
