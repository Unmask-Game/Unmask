using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject vrPlayerPrefab;

    public GameObject vrSpot;
    public List<GameObject> desktopSpots;

    private void Start()
    {
        var index = 0;
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.IsLocal)
            {
                if (player.Value.IsMasterClient)
                {
                    PhotonNetwork.Instantiate(vrPlayerPrefab.name, vrSpot.transform.position, Quaternion.identity);
                }
                else
                {
                    PhotonNetwork.Instantiate(playerPrefab.name, desktopSpots[index].transform.position,
                        Quaternion.identity);
                }
            }

            if (!player.Value.IsMasterClient)
            {
                index++;
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