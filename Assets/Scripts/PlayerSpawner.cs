using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject vrPlayerPrefab;

    public GameObject desktopSpot;
    public List<GameObject> vrSpots;

    private void Start()
    {
        GameObject vrSpawn = vrSpots[Random.Range(0, vrSpots.Count - 1)];
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.IsLocal)
            {
                if (player.Value.IsMasterClient)
                {
                    Vector3 randomOffset = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
                    PhotonNetwork.Instantiate(vrPlayerPrefab.name, vrSpawn.transform.position + randomOffset, Quaternion.identity);
                }
                else
                {
                    PhotonNetwork.Instantiate(playerPrefab.name, desktopSpot.transform.position,
                        Quaternion.identity);
                }
            }
        }

        /*Vector3 spawnPoint;
        if (PhotonNetwork.LocalPlayer.ActorNumber == 1)
        {
            spawnPoint = vrSpawnPoint.transform.position;
        } else
        {
            spawnPoint = desktopSpots.transform.position;
        }
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint, Quaternion.identity);
        */
    }

    void Update()
    {
    }
}