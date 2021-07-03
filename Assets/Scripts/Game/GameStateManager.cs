using DefaultNamespace;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameStateManager
{
    private static GameStateManager _instance = new GameStateManager();
    public bool hasEnded;

    public static GameStateManager Instance
    {
        get { return _instance; }
    }

    private int collectedMasks;

    private void Start()
    {
    }

    public void StartGame()
    {
        hasEnded = false;
        collectedMasks = 0;
    }

    public void EndGame(bool vrHasWon)
    {
        hasEnded = true;
        var policePlayers = GameObject.FindGameObjectsWithTag(Tags.PoliceTag);
        foreach (var player in policePlayers)
        {
            player.GetComponent<GameOverScript>().ActivateGameOverScreen(vrHasWon);
        }

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject.FindGameObjectWithTag(Tags.XrRig).GetComponent<GameOverScript>()
                .ActivateGameOverScreen(vrHasWon);
        }
        Cursor.lockState = CursorLockMode.None;
    }

    public void MaskCollected()
    {
        Debug.Log("Thief collected a mask!");
        collectedMasks++;
        if (GetCollectedMasksPercentile() >= 1)
        {
            EndGame(true);
            Debug.Log("Thief collected enough masks to win");
        }
    }

    public float GetCollectedMasksPercentile()
    {
        return (float)collectedMasks / Constants.MasksNeeded;
    }
}