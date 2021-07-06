using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> itemPool;

    void Start()
    {
        // Only spawn items for Desktop players (as the VR player is always the master client)
        // These items are client side only (every player has his own 4 items)
        if (!PhotonNetwork.IsMasterClient)
        {
            List<GameObject> itemSpots = GameObject.FindGameObjectsWithTag("ItemSpot").ToList();
            itemSpots = itemSpots.OrderBy(rnd => Guid.NewGuid()).ToList();
            // Go through the item pool and instantiate item prefab at a random spot (randomized beforehand)
            for (var i = 0; i < itemPool.Count; i++)
            {
                GameObject itemPrefab = itemPool[i];
                GameObject itemSpot = itemSpots[i];
                Instantiate(itemPrefab, itemSpot.transform.position, Quaternion.identity);
            }
        }
    }
}