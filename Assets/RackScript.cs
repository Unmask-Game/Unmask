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
            racks.transform.GetChild(0).gameObject.SetActive(false);
            var randomRackIndex = random.Next(0, transform.childCount);
            racks.transform.GetChild(randomRackIndex).gameObject.SetActive(true);
        }
    }
}
