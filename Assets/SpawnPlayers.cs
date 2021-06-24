using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject vrSpawnPoint;
    public GameObject desktop1SpawnPoint;

    void Start()
    {
        Vector3 spawnPoint;
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            spawnPoint = vrSpawnPoint.transform.position;
        } else
        {
            spawnPoint = desktop1SpawnPoint.transform.position;
        }
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint, Quaternion.identity);
    }
    
    void Update()
    {
        
    }
}
