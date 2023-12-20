using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.UI.Image;

[RequireComponent(typeof(Animator),typeof(SpriteRenderer), typeof(Collider2D))]
public class CoinPickup : MonoBehaviour {
    private Animator animator;
    private SpriteRenderer sr;
    private Collider2D hitbox;
    public int dupeAmt;
    [SerializeField]
    private Transform direction;
    [SerializeField]
    private RuntimeAnimatorController ctrl;
    [SerializeField]
    private AudioClip coinCollect;
    protected virtual void Start() {
        if (!animator)
            animator = GetComponent<Animator>();
        if (!sr)
            sr = GetComponent<SpriteRenderer>();
        if (!hitbox)
            hitbox = GetComponent<Collider2D>();
        animator.runtimeAnimatorController = ctrl;
        sr.color = new Color(1,.5f,.5f);
        sr.sortingLayerName = "Item";


        // Duplicate Rings
        if (direction == null) return;
        Vector3 pos = gameObject.transform.position;
        for (int i = 0; i < dupeAmt; i++) {
            pos += direction.localPosition;
            GameObject temp = Instantiate(gameObject, pos, Quaternion.identity);
            temp.transform.SetParent(transform.parent);
            temp.GetComponent<CoinPickup>().dupeAmt = 0;
        }
    }

    void Update() {
    }
    protected virtual void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;
        hitbox.isTrigger = true;
        hitbox.enabled = false;
        GameManager.Instance.lives += 1;
        animator.Play("Pickup");
        GameManager.Instance.playSound(coinCollect);
    }
    private void Destroy() {
        Destroy(gameObject);
    }
}
