using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Rendering;

public class NpcSpawner : MonoBehaviour
{
    [SerializeField] private GameObject npcPrefab;

    private List<BoxCollider> _floorTiles;
    private List<BoxCollider> _shopFloorTiles;
    private Queue<NpcController> _npcControllers = new Queue<NpcController>();



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

        for (int i = Constants.NpcCount - 1; i >= 0; i--)
        {
            if (floorTiles.Count == 0) return;
            int randomIndex = Random.Range(0, floorTiles.Count);
            BoxCollider collider = floorTiles[randomIndex];
            GameObject npc = PhotonNetwork.Instantiate(npcPrefab.name, RandomPointInBounds(collider.bounds), Quaternion.Euler(0, Random.Range(0, 360), 0));
            npc.GetComponent<NpcController>().NpcSpawner = this;
            _npcControllers.Enqueue(npc.GetComponent<NpcController>());
            floorTiles.RemoveAt(randomIndex);
        }
    }

    public Vector3 RandomShopFloorTile(Vector3 position)
    {
        // Flip a weighted coin and use a smaller radius if false
        int searchRadius = Random.Range(0, 100) < Constants.NpcChangeShopProbability ? 9999 : 10;
        List<BoxCollider> nearbyFloorTiles =
            _shopFloorTiles.FindAll(bc => Vector3.Distance(bc.transform.position, position) < searchRadius).ToList();
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

    void FixedUpdate()
    {
        if (_npcControllers.Count > 0 && !GameStateManager.Instance.hasEnded)
        {
            NpcController npc = _npcControllers.Dequeue();
            npc.SyncPosition();
            _npcControllers.Enqueue(npc);
        }
    }
}