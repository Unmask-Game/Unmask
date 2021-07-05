using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject vrPlayerPrefab;

    public GameObject desktopSpot;
    public List<GameObject> vrSpots;
    public GameObject xrRig;

    private void Start()
    {
        // Spawn players on correct spawn spots
        GameObject vrSpawn = vrSpots[Random.Range(0, vrSpots.Count - 1)];
        foreach (var player in PhotonNetwork.CurrentRoom.Players)
        {
            // Find own player
            if (player.Value.IsLocal)
            {
                // Is VR player?
                if (player.Value.IsMasterClient)
                {
                    xrRig.transform.position = vrSpawn.transform.position;
                    xrRig.transform.rotation = vrSpawn.transform.rotation;

                    PhotonNetwork.Instantiate(vrPlayerPrefab.name, vrSpawn.transform.position, vrSpawn.transform.rotation);
                }
                else
                {
                    Vector3 randomOffset = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
                    PhotonNetwork.Instantiate(playerPrefab.name, desktopSpot.transform.position + randomOffset,
                        desktopSpot.transform.rotation);
                }
            }
        }
    }
}