using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class MenuManager : MonoBehaviour {
    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button settingsButton;
    [SerializeField]
    private Button stopButton;
    [SerializeField]
    private string title;
    private int randomIndex;
    [SerializeField]
    private TMP_Text sonicTitleRef;
    private Animator anim;

    IEnumerator Start() {
        anim = GetComponent<Animator>();

        if (playButton)
            playButton.onClick.AddListener(() => GameManager.Instance.startGame());
        if (stopButton)
            stopButton.onClick.AddListener(() => GameManager.Instance.quitGame());
        while(true) {
            sonicTitleRef.text = toSpriteText(title);
            yield return new WaitForSeconds(1 / (2*anim.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate));
        }
    }

    string toSpriteText(string text) {
        return string.Join("", text.ToCharArray(0, text.Length)
            .Select((e,i) => "<sprite=" + ((i>=randomIndex?Random.Range(0,26) : (int)e - (int)'a') + 26).ToString() + ">"));
    }

    IEnumerator triggerText() {
        while (randomIndex < title.Length) {
            randomIndex++;
            yield return new WaitForSeconds(1/anim.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate);
        }
    }
}

/* For <@167404425877389312> VGA
public class FoodPickup :MonoBehaviour {
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private float pickupRadius = 2f;
    [SerializeField]
    private int layerMask = 0;
    [SerializeField]
    private Camera cam;

    private void Start() {
        cam = GetComponent<Camera>();
        layerMask += 1 << LayerMask.NameToLayer("Food");
    }
    private void Update() {
        if (!Input.GetButton("Fire1")) return; // We are not pressing the button
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
        RaycastHit[] allStorage = Physics.RaycastAll(ray, pickupRadius,layerMask);

        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (RaycastHit hit in allStorage) {
            Vector3 diff = hit.collider.gameObject.transform.position - position;
            float curDistance = diff.sqrMagnitude;

            if (curDistance < distance) {
                closest = hit.collider.gameObject;
                distance = curDistance;
            }
        }
        PickupObject focus = closest.GetComponent<PickupObject>();
        focus.Pickup();
    }
}
*/