using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private SettingsMenuManager _settings;

    [SerializeField]
    private LobbyMenuManager _lobby;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateRoom()
    {
        // Destroy(this.gameObject);
        LobbyMenuManager lobby = Instantiate(_lobby);
        lobby.parentMenu = this.gameObject;
        lobby.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    public void ShowSettings()
    {
        // Destroy(this.gameObject);
        SettingsMenuManager settings = Instantiate(_settings);
        settings.parentMenu = this.gameObject;
        settings.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
