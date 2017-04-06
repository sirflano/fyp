using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour {

    public string scene;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //If the object collides with a player bullet object load the scene specified in the Unity inspector
        if(other.gameObject.layer == 9)
        {
            SceneManager.LoadScene(scene);
        }
    }
}
