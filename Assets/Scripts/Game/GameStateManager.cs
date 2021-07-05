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

    public void StartGame()
    {
        hasEnded = false;
        collectedMasks = 0;
    }

    // Infinity War
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
        collectedMasks++;
        if (GetCollectedMasksPercentile() >= 1)
        {
            // Thief wins the game
            Debug.Log("Thief collected enough masks to win");
            EndGame(true);
        }
    }

    public float GetCollectedMasksPercentile()
    {
        return (float)collectedMasks / Constants.MasksNeeded;
    }
}