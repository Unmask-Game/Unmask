using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCanvas : MonoBehaviour
{
    public Transform toy;
    List<GameObject> generatedObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickSpawnCube()
    {
        Debug.Log("Button pressed");
        Transform newToy = Instantiate(toy);
        newToy.position += new Vector3(0, 1, -2);
        generatedObjects.Add(newToy.gameObject);
    }

    public void OnClickReset()
    {
        foreach (var obj in generatedObjects)
        {
            Destroy(obj);
        }
    }
}
