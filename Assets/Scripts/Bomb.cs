using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bomb : MonoBehaviour {
    public float fuseTime;
    public float blastRadius;
    public float blastForce;
    public float verticalForce;

    public Material lit;
    public Color explodeColor;


    public void Thrown() {
        StartCoroutine(Fuse());
    }

    IEnumerator Fuse() {
        Renderer renderer = GetComponent<Renderer>();
        transform.DOScale(transform.localScale * 1.2f, fuseTime);
        renderer.material = lit;
        Color old = renderer.material.GetColor("_BaseColor");
        DOTween.To(()=>old,x=>renderer.material.SetColor("_BaseColor",x),explodeColor,fuseTime);
        
        yield return new WaitForSeconds(fuseTime);
        
        
        List<Collider> hit = new List<Collider>(Physics.OverlapSphere(transform.position, blastRadius));
        foreach(Collider c in hit) {
            if (c.tag == "Player") {
                c.GetComponent<FriendController>().KnockDown(blastForce / 20f);
            }
            if (c.GetComponent<Rigidbody>() != null && c != GetComponent<Collider>()) {
                 Vector3 dir = (c.transform.position - transform.position).normalized;
                dir *= blastForce;
                dir += new Vector3(0,verticalForce,0);
                c.GetComponent<Rigidbody>().AddForce(dir);
            }
        }
       Destroy(gameObject);
    }
}
