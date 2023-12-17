using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MonitorPickup : MonoBehaviour {
    [RequireComponent(typeof(Animator),typeof(SpriteRenderer))]
    private class Display : MonoBehaviour {
        private void Destroy() {
            Destroy(gameObject);
        }
    }
    enum Type {
        None,
        Invincible,
        Coin,
        Collectable
    }
    [SerializeField]
    private Type forced;
    [SerializeField]
    private RuntimeAnimatorController ctrl;
    [Header("Sprites")]
    [SerializeField]
    private Sprite invincibleSprite;
    [SerializeField]
    private Sprite coinSprite;
    [SerializeField]
    private Sprite collectableSprite;

    private Animator anim;
    private SpriteRenderer sr;
    private Type type;
    
    void Start() {
        if (forced != Type.None)
            type = forced;
        else
            InitRandomType();
        InitMonitor();
    }
    private void InitMonitor() {
        if (!sr || !anim) {
            GameObject obj = new GameObject("Display", typeof(Display));
            obj.transform.SetParent(gameObject.transform);
            obj.transform.localPosition = new Vector3(0, .07f, 0);
            if (!sr) {
                sr = obj.GetComponent<SpriteRenderer>();
                sr.sortingLayerName = "Item";
                sr.sortingOrder = 1;
            }
            if (!anim)
                anim = obj.GetComponent<Animator>();
        }
        switch (type) {
            case Type.Invincible:
                sr.sprite = invincibleSprite;
                break;
            case Type.Coin:
                sr.sprite = coinSprite;
                break;
            case Type.Collectable:
                sr.sprite = collectableSprite;
                break;
            default:
                anim.Play("Unknown");
                break;
        }
        Debug.Log("Created " + type + " monitor pickup");
        anim.runtimeAnimatorController = ctrl;
    }
    private void InitRandomType() {
        Array values = Enum.GetValues(typeof(Type));
        type = (Type) values.GetValue(UnityEngine.Random.Range(1, values.Length ));
    }

    // Update is called once per frame
    void Update() {
            
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player") || !anim) return;
        anim.Play("Using");
        PlayerCtrl focus = other.gameObject.GetComponent<PlayerCtrl>();
        switch (type) {
            case Type.Coin:
                GameManager.Instance.lives += 5;
                break;
            case Type.Invincible:
                focus.invincible();
                break;
            case Type.Collectable:
                GameManager.Instance.score += 1000;
                break;
        }
    }
}
