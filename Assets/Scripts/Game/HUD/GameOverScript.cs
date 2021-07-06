using System;
using System.Collections;
using DefaultNamespace;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    [SerializeField] private GameObject hud;
    [SerializeField] private GameObject gameOverScreen;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    public void ActivateGameOverScreen(bool vrHasWon)
    {
        if (!GetComponent<PhotonView>() || GetComponent<PhotonView>().IsMine)
        {
            // VR Player is always master client
            if (PhotonNetwork.IsMasterClient == vrHasWon)
            {
                GameObject.FindWithTag("Global").transform.Find("VictoryAudio").GetComponent<AudioSource>().Play();
            }
            else
            {
                GameObject.FindWithTag("Global").transform.Find("DefeatAudio").GetComponent<AudioSource>().Play();
            }

            hud.SetActive(false);
            gameOverScreen.SetActive(true);
            if (vrHasWon)
            {
                gameOverScreen.transform.Find("ThiefVictory").gameObject.SetActive(true);
            }
            else
            {
                gameOverScreen.transform.Find("PoliceVictory").gameObject.SetActive(true);
            }

            StartCoroutine(LoadLobbyScene());
        }
    }

    IEnumerator LoadLobbyScene()
    {
        // Stop game
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(Constants.WaitAfterGameOver);
        if (PhotonNetwork.IsMasterClient)
        {
            // VR Player returns to the VR Room
            SceneManager.LoadScene(2);
        }
        else
        {
            // Desktop Player returns to the normal room
            SceneManager.LoadScene(4);
        }
    }
}