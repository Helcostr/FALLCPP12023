using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {
    SpriteRenderer sr;

    public float projectileSpeed;
    public Transform spawnPointRight;
    public Transform spawnPointLeft;

    public Projectile projectilePrefab;

    void Start() {
        sr = GetComponent<SpriteRenderer>();
        if (projectileSpeed <= 0) projectileSpeed = 7.0f;
        if (!spawnPointRight || !spawnPointLeft || !projectilePrefab)
            Debug.Log("Please set default values on " + gameObject.name);
    }

    public void Fire() {
        Transform focusSpawn = sr.flipX ? spawnPointLeft : spawnPointRight;
        Projectile focus = Instantiate(projectilePrefab, focusSpawn.position, focusSpawn.rotation);
        focus.speed = projectileSpeed * (sr.flipX ? -1 : 1);
    }
}
