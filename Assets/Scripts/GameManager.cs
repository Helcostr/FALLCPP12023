using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public int score;

    [SerializeField]
    private PickupPrefabsClass PickupPrefabs;
    
    [System.Serializable]
    private class PickupPrefabsClass {
        public GameObject powerup;
        public GameObject life;
        public GameObject score;
        public GameObject invincible;
        public GameObject speed;
    }

    void Awake() {
        if (instance != null) Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);

        Random.InitState((int)System.DateTime.Now.Ticks);
    }

    void Start() {
        pickupSpawner();
    }

    // Update is called once per frame
    void Update() {
        
    }

    void pickupSpawner() {
        Debug.Log("Get spawner");
        // Obtain Spawn list
        GameObject[] pickupSpawnpoints = GameObject.FindGameObjectsWithTag("PickupSpawner");
        if (pickupSpawnpoints.Length == 0) return; // Skip because no spawnpoints
        Debug.Log("Get Prefabs");
        // Accumulate all the prefabs
        List<GameObject> allPickupPrefabs = new List<GameObject> { PickupPrefabs.powerup, PickupPrefabs.life,
            PickupPrefabs.score, PickupPrefabs.invincible, PickupPrefabs.speed };
        // allPickupPrefabs.RemoveAll(x => x != null); // Null Check

        if (allPickupPrefabs.Count == 0) return; // Skip because no pickupPrefabs set

        Debug.Log("Spawning");
        foreach (GameObject prefab in allPickupPrefabs) {
            Debug.Log(prefab.name);
        }
        foreach (GameObject spawnpoint in pickupSpawnpoints)
            Instantiate(allPickupPrefabs[Random.Range(0, allPickupPrefabs.Count)], spawnpoint.transform.position, spawnpoint.transform.rotation);
    }
}
