using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyMenuManager : MonoBehaviour
{

    public GameObject parentMenu;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Quit()
    {
        Destroy(this.gameObject);
        parentMenu.SetActive(true);
    }
}
