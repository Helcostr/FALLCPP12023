using UnityEngine;

[RequireComponent(typeof(Animator),typeof(SpriteRenderer))]
public class Motora_Smoke : MonoBehaviour {
    private Vector3 endPos;
    private SpriteRenderer sr;
    private Animator anim;
    
    public void init(RuntimeAnimatorController ctrl, float smoke_distance) {
        endPos = transform.position + Vector3.up * smoke_distance;
        sr = GetComponent<SpriteRenderer>();
        sr.sortingLayerName = "Enemy";
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = ctrl;
    }

    // Update is called once per frame
    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime/3);
        sr.color = Color.white * Mathf.Lerp(sr.color.a, 0, Time.deltaTime);
        anim.SetFloat("transparency", sr.color.a);

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Poof"))
            Destroy(gameObject);
    }
}
