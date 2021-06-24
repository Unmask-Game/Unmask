using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    
    void Start()
    {
        Vector2 randomPosition = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        PhotonNetwork.Instantiate(playerPrefab.name, randomPosition, Quaternion.identity);
    }
    
    void Update()
    {
        
    }
}
