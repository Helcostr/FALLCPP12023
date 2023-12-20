using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beeton : Enemy {
    // Start is called before the first frame update
    private Coroutine isShooting;
    [SerializeField]
    private float range;
    [SerializeField]
    private GameObject muzzleFlash;
    [SerializeField]
    private RuntimeAnimatorController bulletAnim;
    [SerializeField]
    private float bulletSpeed;
    protected override void Start() {
        base.Start();
        muzzleFlash.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if (Mathf.Abs(GameManager.Instance.playerInstance.transform.position.x - transform.position.x) < range)
            if (isShooting == null)
                isShooting = StartCoroutine(shoot());
    }
    IEnumerator shoot() {
        Quaternion rotation = Quaternion.identity;
        bool isFlipped = GameManager.Instance.playerInstance.transform.position.x > transform.position.x;
        rotation.eulerAngles = new Vector3(0, isFlipped ? 180 : 0, 0);
        transform.rotation = rotation;
        anim.Play("Fire");
        yield return new WaitForSeconds(.5f);
        muzzleFlash.SetActive(true);
        
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        GameObject bulletChild = new GameObject("BeetonBullet", typeof(BeetonBullet));
        bulletChild.transform.position = muzzleFlash.transform.position;
        bulletChild.GetComponent<BeetonBullet>().init(bulletAnim, isFlipped, bulletSpeed);
        muzzleFlash.SetActive(false);
        yield return new WaitForSeconds(.5f);
        anim.Play("Idle");
        yield return new WaitForSeconds(1f);
        isShooting = null;
    }
    public override void kill() {
        base.kill();
        if (isShooting != null) StopCoroutine(isShooting);
    }
}
