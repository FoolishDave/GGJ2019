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
    public int playerNum = 0;
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
    private ParticleSystem runParticle;
    // Input variables uwu
    private bool jump;
    private bool interact;
    private bool leave;
    private bool throwHeld;
    public bool knockedDown;
    public bool pickedUp;
    private float knockdownTimer;

    private void Awake() {
        rigid = GetComponent<Rigidbody>();
        runParticle = GetComponentInChildren<ParticleSystem>();
    }

    void Start() {
        player = ReInput.players.GetPlayer(playerId);
        Color c = GetComponent<Renderer>().material.GetColor("_BaseColor");
        c.a = .5f;
        GetComponentInChildren<ParticleSystemRenderer>().material.SetColor("_TintColor", c);
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
            KnockDown(collision.impulse.sqrMagnitude);
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

        if (pickedUp) {
            if (jump && Random.Range(0f,1f) > .9f) {
                transform.parent = transform.parent.parent.parent;
                pickedUp = false;
                rigid.isKinematic = false;
                Standup();
            }
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
        if (Vector3.Angle(transform.forward, movementVector) > 5) {
            Vector3 angular = rigid.angularVelocity;
            angular.y = -rotateAmount * rotateSpeed;
            rigid.angularVelocity = angular;
        } else {
            rigid.angularVelocity = Vector3.zero;
        }
        float moveSpeed = movementVector.magnitude;
        if (moveSpeed < 0f) moveSpeed *= -1f;
        if (moveSpeed > 1f) moveSpeed = 1f;
        ParticleSystem.EmissionModule mod = runParticle.emission;
        mod.rateOverTime = moveSpeed * 75f;
        Vector3 movePos = transform.position + movementVector.normalized * moveSpeed * speed;
        movePos.y = transform.position.y;
        rigid.MovePosition(movePos);

        if (pickupPoint.transform.childCount == 0) {
            holding = null;
        }

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
        if (holding != null) return;
        IEnumerable<Collider> colliders = Physics.OverlapBox(transform.position + transform.forward * pickupOffset, pickupBox).Where(x => x.transform != transform && (x.tag == "Pickup" || x.tag == "Player"));
        if (!colliders.Any()) return;
        Collider pickupTarget = colliders.Aggregate((i1, i2) => Vector3.Distance(i1.transform.position, transform.position) < Vector3.Distance(i2.transform.position, transform.position) ? i1 : i2);
        if (pickupTarget != null) {
            if (pickupTarget.tag == "Player" && !pickupTarget.GetComponent<FriendController>().knockedDown) {
                return;
            } else if (pickupTarget.tag == "Player") {
                pickupTarget.GetComponent<FriendController>().pickedUp = true;
            }
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
        if (holding.tag == "Player") {
            FriendController otherPlayer = holding.GetComponent<FriendController>();
            otherPlayer.pickedUp = false;
            otherPlayer.knockdownTimer = knockdownTime;
        }
        if (holding.GetComponent<Bomb>() != null) {
            holding.GetComponent<Bomb>().Thrown();
        }
        holding = null;
        Vector3 throwVector = transform.forward.normalized;
        throwVector = Quaternion.AngleAxis(throwAngle, transform.right) * throwVector * throwForce;
        heldRigid.AddForce(throwVector);
        
    }

    public void Respawn() {
        rigid.velocity = Vector3.zero;
        transform.position = new Vector3(Random.Range(-2.5f, 2.5f), 4, Random.Range(-1f, 1f));
        KnockDown(.5f);
        transform.eulerAngles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
    }

    public void Standup() {
        rigid.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        knockedDown = false;
        knockdownTimer = knockdownCooldown;
        transform.DOMoveY(transform.position.y + .2f, .2f);
        transform.DORotate(Vector3.zero, .3f);
    }

    public void KnockDown(float impulse) {
        Debug.Log("Knocked down.");
        rigid.constraints = RigidbodyConstraints.None;
        knockedDown = true;
        knockdownTimer = knockdownTime * impulse;
        if (holding != null) {
            holding.GetComponent<Rigidbody>().isKinematic = false;
            holding.transform.parent = transform.parent;
            if (holding.tag == "Player") {
                FriendController otherPlayer = holding.GetComponent<FriendController>();
                otherPlayer.pickedUp = false;
                otherPlayer.Standup();
            }
            holding = null;
        }
    }
}
