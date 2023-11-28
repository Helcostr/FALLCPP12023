using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyTurret : Enemy {
    public float projectileFireRate;

    float timeSinceLastFire = 0;
    public float detectRange = 2f;
    public LayerMask layerMask = 0;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        if (projectileFireRate <= 0)
            projectileFireRate = 2;
        layerMask = 1 << LayerMask.NameToLayer("Player");
    }

    // Update is called once per frame
    void Update() {
        AnimatorClipInfo[] curPlayingClips = anim.GetCurrentAnimatorClipInfo(0);

        if (curPlayingClips[0].clip.name != "Fire") {
            if (Time.time >= timeSinceLastFire + projectileFireRate) {
                Collider2D player = Physics2D.OverlapCircle(transform.position, detectRange, layerMask);
                if (player != null) {
                    anim.SetTrigger("Fire");
                    timeSinceLastFire = Time.time;
                    sr.flipX = player.gameObject.transform.position.x < transform.position.x;
                }


            }
        }
    }
}