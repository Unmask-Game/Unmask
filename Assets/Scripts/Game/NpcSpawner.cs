using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class NpcSpawner : MonoBehaviour
{
    [SerializeField] private GameObject npcPrefab;
    [SerializeField] private int npcCount;

    private List<BoxCollider> _floorTiles;
    private List<BoxCollider> _shopFloorTiles;

    private static NpcSpawner _instance;

    public static NpcSpawner Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
            gameObject.SetActive(false);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        List<GameObject> floorTiles = GameObject.FindGameObjectsWithTag("Floor").ToList();
        _floorTiles = floorTiles.Select(f => f.GetComponent<BoxCollider>()).ToList();

        List<GameObject> shopFloorTiles = GameObject.FindGameObjectsWithTag("ShopFloor").ToList();
        _shopFloorTiles = shopFloorTiles.Select(f => f.GetComponent<BoxCollider>()).ToList();
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPlayers();
        }
    }

    void SpawnPlayers()
    {
        List<BoxCollider> floorTiles = new List<BoxCollider>();
        floorTiles.AddRange(_floorTiles);
        floorTiles.AddRange(_shopFloorTiles);

        for (int i = npcCount - 1; i >= 0; i--)
        {
            if (floorTiles.Count == 0) return;
            int randomIndex = Random.Range(0, floorTiles.Count);
            BoxCollider collider = floorTiles[randomIndex];
            PhotonNetwork.Instantiate(npcPrefab.name, RandomPointInBounds(collider.bounds), Quaternion.Euler(0, Random.Range(0, 360), 0));
            floorTiles.RemoveAt(randomIndex);
        }
    }

    public Vector3 RandomShopFloorTile(Vector3 position)
    {
        List<BoxCollider> nearbyFloorTiles =
            _shopFloorTiles.FindAll(bc => Vector3.Distance(bc.transform.position, position) < 12).ToList();
        BoxCollider collider = nearbyFloorTiles[Random.Range(0, nearbyFloorTiles.Count)];
        return RandomPointInBounds(collider.bounds);
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x + 0.7f, bounds.max.x - 0.7f),
            bounds.max.y,
            Random.Range(bounds.min.z + 0.7f, bounds.max.z - 0.7f)
        );
    }

    void Update()
    {
    }
}