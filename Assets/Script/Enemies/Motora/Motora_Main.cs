using System.Collections;
using System.Linq;
using UnityEngine;


public class Motora_Main : Enemy {
    [SerializeField]
    private float speed = 1f;
    [SerializeField]
    private float smoke_delay = .5f;
    [SerializeField]
    private float smoke_dist = 1f;
    [SerializeField]
    private Transform smoke_spawn;
    [SerializeField]
    private RuntimeAnimatorController mainAnimation;
    [SerializeField]
    private RuntimeAnimatorController smokeAnmiation;
    private bool goingLeft = true;
    
    [SerializeField]
    private Transform[] boundry;
    private float left;
    private float right;
    private Coroutine smokeCoroutine;
    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        anim.runtimeAnimatorController = mainAnimation;
        rb.freezeRotation = true;
        float[] boundryFloats = boundry.Select(each => {
            return each.position.x;
        }).ToArray<float>();
        right = Mathf.Max(boundryFloats);
        left = Mathf.Min(boundryFloats);

        smokeCoroutine = StartCoroutine(spawnSmoke());
    }

    IEnumerator spawnSmoke() {
        while (true) {
            yield return new WaitForSeconds(smoke_delay);
            GameObject obj = new GameObject("Smoke", typeof(Motora_Smoke));
            obj.transform.position = smoke_spawn.position;
            obj.GetComponent<Motora_Smoke>().init(smokeAnmiation, smoke_dist);
        }
    }

    // Update is called once per frame
    void Update() {
        if (isDead) return;
        rb.velocity = goingLeft ? new Vector2(-speed, rb.velocity.y) : new Vector2(speed, rb.velocity.y);

        float x = transform.position.x;
        
        if (x < left) {
            goingLeft = false;
            Quaternion rotation = Quaternion.identity;
            rotation.eulerAngles = new Vector3(0, 180, 0);
            transform.rotation = rotation;
        }
        if (x > right) {
            goingLeft = true;
            Quaternion rotation = Quaternion.identity;
            rotation.eulerAngles = new Vector3(0, 0, 0);
            transform.rotation = rotation;
        }
    }

    public override void kill() {
        base.kill();
        if (smokeCoroutine != null) StopCoroutine(smokeCoroutine);
    }
}
