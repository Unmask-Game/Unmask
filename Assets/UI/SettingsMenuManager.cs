using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuManager : MonoBehaviour
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

    public void Done()
    {
        Destroy(this.gameObject);
        parentMenu.SetActive(true);
    }
}
