using DefaultNamespace;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager _instance = new GameStateManager();
    public static GameStateManager Instance { get { return _instance; } }

    private PhotonView _view;

    private int collectedMasks;

    // Start is called before the first frame update
    void Start()
    {
        _view = GetComponent<PhotonView>();
    }

    public void StartGame()
    {
        collectedMasks = 0;
    }

    public void MaskCollected()
    {
        Debug.Log("Thief collected a mask!");
        collectedMasks++;
        if (GetCollectedMasksPercentile() >= 1)
        {
            // TODO show vr player victory scene
            Debug.Log("Thief collected enough masks to win");
        }
    }

    public float GetCollectedMasksPercentile()
    {
        return (float)collectedMasks / Constants.MasksNeeded;
    }
}
