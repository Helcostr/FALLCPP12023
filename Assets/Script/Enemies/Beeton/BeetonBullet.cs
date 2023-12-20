using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator),typeof(SpriteRenderer))]
public class BeetonBullet : MonoBehaviour {
    private float speed;
    private SpriteRenderer sr;
    public void init(RuntimeAnimatorController ctrl, bool flipped, float speed) {
        sr = GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "Enemy";
        GetComponent<Animator>().runtimeAnimatorController = ctrl;
        sr.flipX = flipped;
        this.speed = speed;
        Destroy(gameObject, 10f);
    }

    // Update is called once per frame
    void Update() {
        transform.position = Vector2.MoveTowards(transform.position, transform.position + new Vector3(sr.flipX ? 100 : -100, -100,0), Time.deltaTime * speed);
        if (Physics2D.OverlapCircle(transform.position, .1f, 1 << LayerMask.NameToLayer("Player"))) {
            GameManager.Instance.playerInstance.dmg(1);
            Destroy(gameObject);
        }

    }
}
