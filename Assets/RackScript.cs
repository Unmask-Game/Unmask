using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;


public class RackScript : MonoBehaviour
{
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            System.Random random = new System.Random();
            GetComponent<PhotonView>().RPC("randomizeRacks", RpcTarget.All, random.Next());
        }
    }

    [PunRPC]
    private void randomizeRacks(int seed)
    {
        Debug.Log(seed);
        System.Random random = new System.Random(seed);
        foreach (Transform racks in transform)
        {
            var randomRackIndex = random.Next(0, racks.childCount - 1);
            racks.transform.GetChild(randomRackIndex).gameObject.SetActive(true);
        }
    }
}
