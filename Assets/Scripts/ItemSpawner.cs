using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> itemPool;
    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> itemSpots = GameObject.FindGameObjectsWithTag("ItemSpot").ToList();
        itemSpots = itemSpots.OrderBy(rnd => Guid.NewGuid()).ToList();
        for (var i = 0; i < itemPool.Count; i++)
        {
            GameObject itemPrefab = itemPool[i];
            GameObject itemSpot = itemSpots[i];
            Instantiate(itemPrefab, itemSpot.transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}